using BlazorBootstrap;
using Project.IO.Utilities;
using Project.IO.Classes;
using System.Diagnostics;
using Project.IO.Classes.Service;
using Project.IO.Classes.Model;
using Microsoft.AspNetCore.Components;

namespace Project.IO.Components.Pages
{
    public partial class MemberOverviewPage : ComponentBase
    {

        private Modal memberModal = default!;
        [Inject]
        private AccountUtil accountUtil {get;set;} = default!;
        [Inject]
        private ProjectUtil projectUtil { get; set; } = default!;
        [Inject]
        private RoleService roleService { get; set; } = default!;
        [Inject]
        private ProjectAssignmentService projectService { get; set; } = default!;
        [Inject]
        private NavigationManager Navigation {get;set;} = default!;
        private List<Role> roles = [];
        private string? chosenUser;
        private int chosenRole;

        private List<(int AssignmentId,string Username, string Rolename)> memberRoles = [];

        private List<ProjectAssignment> members = [];

        private ProjectModel currentProject;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                // Fetch the Role data
                await GetProjectMembers();
                await GetProjectRoles();
                await LoadMemberList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading roles: {ex.Message}");
            }
        }
        private async Task LoadMemberList()
        {
            memberRoles = [];
            foreach (ProjectAssignment member in members)
            {
                string username = await GetMemberNameAsync(member.UserId);
                string rolename = await GetRoleNameAsync(member.RoleId);
                memberRoles.Add((member.Id, username, rolename));
            }
        }
        private async void ShowAddMemberModal()
        {
            try
            {
                currentProject = await ShowCurrentProject();
                if (currentProject == null)
                {
                    Debug.WriteLine("Projects list is null after fetching.");
                }
                else
                {
                    Debug.WriteLine($"Fetched {currentProject} projects.");
                }

                await memberModal.ShowAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error fetching projects: {ex.Message}");
            }
        }

        private async Task<ProjectModel> ShowCurrentProject()
        {
            var project = await projectUtil.GetCurrentProject();

            if (project == null)
            {
                Debug.WriteLine("Project is null in ShowCurrentProject method.");
            }
            else
            {
                Debug.WriteLine($"ShowCurrentProject fetched {project} project.");
            }

            return project;
        }

        private async Task addUserToProject()
        {
            if (!(IsEmpty(chosenUser) || chosenRole.Equals(null)))
            {
                Member user = await accountUtil.getMemberByEmail(chosenUser);
                Role role = await roleService.GetRoleById(chosenRole);
                if (user!=null)
                {
                    await projectService.AddNewParticipant(user.Id, role.Id);
                    await GetProjectMembers();
                    await LoadMemberList();
                    ClearInputMenu();
                    await memberModal.HideAsync();
                }
            }
        }

        private void ClearInputMenu()
        {
            chosenUser = string.Empty;
            chosenRole = 0;
        }

        private bool IsEmpty(string value)
        {
            bool result = false;

            if (value == null || value == "")
            {
                result = true;
            }

            return result;
        }

        private async Task<List<ProjectAssignment>> GetProjectMembers()
        {

            members = await projectService.GetProjectAssignments();
            if (members == null)
            {
                Debug.WriteLine("Member list is null in GetAllMembersInProject method.");
            }
            else
            {
                Debug.WriteLine($"Get members fetched {members.Count} members.");
            }
            return members;
        }
        private async Task<List<Role>> GetProjectRoles()
        {
            roles = await roleService.getRolesForProject();
            if (roles == null)
            {
                Debug.WriteLine("Role list is null in GetAllRolesInProject method.");
            }
            else
            {
                Debug.WriteLine($"Get roles fetched {roles.Count} roles.");
            }

            return roles;
        }

        public void NavigateToProjectMember(int roleId)
        {
            Console.WriteLine(roleId);
            Navigation.NavigateTo($"/editMemberRolePage/{roleId}");
        }
        public string GetMemberName(int userId) => GetMemberNameAsync(userId).GetAwaiter().GetResult();
        public string GetRoleName(int roleId) => GetRoleNameAsync(roleId).GetAwaiter().GetResult();
        private async Task<string> GetMemberNameAsync(int userId)
        {
            Member member = await accountUtil.GetMemberById(userId);
            return member.username;
        }
        private async Task<string> GetRoleNameAsync(int RoleId)
        {
            Role role = await roleService.GetRoleById(RoleId);
            return role.RoleName;
        }
    }
}

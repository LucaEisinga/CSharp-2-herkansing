using BlazorBootstrap;
using Project.IO.Utilities;
using Project.IO.Classes;
using System.Diagnostics;
using Project.IO.Classes.Service;
using Project.IO.Classes.Model;
using Microsoft.AspNetCore.Components;

namespace Project.IO.Components.Pages
{
    public partial class MemberOverviewPage
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
        private string? chosenUser;
        private string? chosenRole;

        private List<ProjectAssignment> members = new List<ProjectAssignment>();

        private ProjectModel currentProject;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                // Fetch the Role data
                await GetProjectMembers();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading roles: {ex.Message}");
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
            if (!(IsEmpty(chosenUser) || IsEmpty(chosenRole)))
            {
                Member user = await accountUtil.getMemberByEmail(chosenUser);
                Role role = await roleService.GetRoleByName(chosenRole);
                if (user!=null)
                {
                    await projectService.AddNewParticipant(user.Id, role.Id);
                    await GetProjectMembers();
                    ClearInputMenu();
                    await memberModal.HideAsync();
                }
            }
        }

        private void ClearInputMenu()
        {
            chosenUser = string.Empty;
            chosenRole = null;
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
                Debug.WriteLine("Role list is null in GetAllMembersInProject method.");
            }
            else
            {
                Debug.WriteLine($"Get members fetched {members.Count} members.");
            }

            return members;
        }

        public void NavigateToProjectMember(int roleId)
        {
            Console.WriteLine(roleId);
            Navigation.NavigateTo($"/editMemberRolePage/{roleId}");
        }

    }
}

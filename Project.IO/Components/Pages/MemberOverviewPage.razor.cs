using BlazorBootstrap;
using Project.IO.Utilities;
using Project.IO.Classes;
using System.Diagnostics;
using System.ComponentModel;
using Project.IO.Classes.Service;
using Project.IO.Classes.Model;

namespace Project.IO.Components.Pages
{
    public partial class MemberOverviewPage
    {

        private Modal memberModal = default!;
        private ProjectUtil projectUtil = new ProjectUtil();
        private RoleService roleService = new RoleService();
        private string? chosenUser;
        private string? chosenRole;

        private List<Role> roles = new List<Role>();

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

        private async Task AddRoleToUser()
        {
            if (IsEmpty(chosenUser) && IsEmpty(chosenRole))
            {
                await roleService.AddRoleToMember(chosenUser, chosenRole);
            }
        }

        private bool IsEmpty(string value)
        {
            bool result = false;

            if (!(value == null) || !(value == ""))
            {
                result = true;
            }

            return result;
        }

        private async Task<List<Role>> GetProjectMembers()
        {

            roles = await roleService.GetAllMembersInProject();

            if (roles == null)
            {
                Debug.WriteLine("Role list is null in GetAllMembersInProject method.");
            }
            else
            {
                Debug.WriteLine($"Get members fetched {roles.Count} members.");
            }

            return roles;
        }

    }
}

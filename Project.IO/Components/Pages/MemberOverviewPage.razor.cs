using BlazorBootstrap;
using Project.IO.Utilities;
using Project.IO.Classes;
using System.Diagnostics;
using System.ComponentModel;
using Project.IO.Classes.Service;

namespace Project.IO.Components.Pages
{
    public partial class MemberOverviewPage
    {

        private Modal memberModal = default!;
        private ProjectUtil projectUtil = new ProjectUtil();
        private RoleService roleService = new RoleService();
        private string? chosenUser;
        private string? chosenRole;

        private ProjectModel currentProject;

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

    }
}

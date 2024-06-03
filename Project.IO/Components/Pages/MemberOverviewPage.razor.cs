using BlazorBootstrap;
using Project.IO.Utilities;
using Project.IO.Classes;
using System.Diagnostics;
using System.ComponentModel;

namespace Project.IO.Components.Pages
{
    public partial class MemberOverviewPage
    {

        private Modal memberModal = default!;
        private ProjectUtil projectUtil = new ProjectUtil();

        private List<ProjectModel> projects = new List<ProjectModel>();

        private async void ShowAddMemberModal()
        {
            try
            {
                projects = await ShowAllProjects();
                if (projects == null)
                {
                    Debug.WriteLine("Projects list is null after fetching.");
                }
                else
                {
                    Debug.WriteLine($"Fetched {projects.Count} projects.");
                }

                // Show the modal
                await memberModal.ShowAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error fetching projects: {ex.Message}");
            }
        }

        private async Task<List<ProjectModel>> ShowAllProjects()
        {
            var projectList = await projectUtil.GetListOfProjects();
            if (projectList == null)
            {
                Debug.WriteLine("Project list is null in ShowAllProjects method.");
            }
            else
            {
                Debug.WriteLine($"ShowAllProjects fetched {projectList.Count} projects.");
            }
            return projectList;
        }

    }
}

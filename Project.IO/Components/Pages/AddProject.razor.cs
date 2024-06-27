using Microsoft.AspNetCore.Components;
using Project.IO.Classes;
using Project.IO.Classes.Service;
using Project.IO.Utilities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Project.IO.Components.Pages
{
    public partial class AddProject
    {
        private ProjectUtil ProjectUtil = new ProjectUtil();

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        private string ProjectTitle { get; set; }
        private string ProjectDescription { get; set; }
        private DateTime ProjectDeadline { get; set; } = DateTime.Now;

        protected override void OnInitialized()
        {
            try
            {
                if (!SessionService.Instance.IsLoggedIn)
                {
                    NavigationManager.NavigateTo("/account");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during initialization: {ex.Message}");
            }
        }

        private async Task CreateProject()
        {
            try
            {
                if (ProjectDeadline < DateTime.Now.Date)
                {
                    await DisplayAlert("Invalid Date", "The project deadline cannot be in the past.", "OK");
                    return;
                }

                await ProjectUtil.createProject(ProjectTitle, ProjectDescription, ProjectDeadline);
                await DisplayAlert("Success", "Project added successfully.", "OK");
                NavigationManager.NavigateTo("/");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating project: {ex.Message}");
                await DisplayAlert("Error", "An error occured while creating the project.", "OK");
            }
        }

        private Task DisplayAlert(string title, string message, string cancel)
        {
            // Implement alert functionality suitable for your platform
            Console.WriteLine($"{title}: {message}");
            return Task.CompletedTask;
        }

        /*private async Task<List<ProjectModel>> GetUserProjects()
        {
            try
            {
                return await ProjectUtil.GetProjectsForLoggedInUser();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting projects: {ex.Message}");
                return new List<ProjectModel>();
            }
        }*/
    }
}

using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using Project.IO.Classes;
using Project.IO.Classes.Service;
using Project.IO.Utilities;
using System.Diagnostics;

namespace Project.IO.Components.Pages
{
    partial class AccountPage
    {

        private Modal modal = default!;
        private Modal errorModal = default!;
        private ProjectUtil _projectUtil = new ProjectUtil();
        private DatabaseUtil databaseUtil = new DatabaseUtil();
        private AccountUtil _accountUtil = new AccountUtil();
        private string? logUser;
        private string? logPassword;
        private string? userName;
        private string? email;
        private string? password;
        private string? repeatPassword;
        private List<ProjectModel> userProjects;

        /*private async Task OnShowModalClick()
        {


                // Show the modal
                await modal.ShowAsync();
            
        }*/

        private async Task OnHideModalClick()
        {
            await modal.HideAsync();
        }

        private void ShowConnection()
        {
            databaseUtil.CreateConnection();
        }

        private async Task LoginUser()
        {
            if (await _accountUtil.canLogin(logUser, logPassword))
            {

                await LoadUserProjects();

                await modal.ShowAsync();
            }
        }

        private async Task LoadUserProjects()
        {
            Debug.WriteLine("Loading user projects...");

            userProjects = await _projectUtil.GetProjectsForLoggedInUser();

            /*if (userProjects != null && userProjects.Count > 0)
            {
                Debug.WriteLine($"Loaded {userProjects.Count} projects.");
            }
            else
            {
                Debug.WriteLine("No projects found for the user");
            }*/
        }

        private void CreateNewUser()
        {
            if (IsEmpty(userName) || IsEmpty(email))
            {
                if (password.Equals(repeatPassword))
                {
                    _accountUtil.RegisterNewUser(userName, email, password, repeatPassword);
                }
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

        

        private void NavigateToAddProject()
        {
            Navigation.NavigateTo("/addProject");
        }

    }
}

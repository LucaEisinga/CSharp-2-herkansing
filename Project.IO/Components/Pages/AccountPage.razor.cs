using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using Project.IO.Classes.Model;
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
        private List<MemberProjectModel> userProjects;

        [Inject]
        private NavigationManager Navigation { get; set; }

        private async Task LoginUser()
        {
            if (await _accountUtil.canLogin(logUser, logPassword))
            {
                await LoadUserProjects();

                await modal.ShowAsync();
                ClearLoginInput();
            }
        }

        private void ClearLoginInput()
        {
            logUser = string.Empty;
            logPassword = string.Empty;
        }

        private async Task LoadUserProjects()
        {
            Debug.WriteLine("Loading user projects...");

            userProjects = await _projectUtil.GetProjectsForLoggedInUser();
        }

        private void CreateNewUser()
        {
            if (IsEmpty(userName) || IsEmpty(email))
            {
                if (password.Equals(repeatPassword))
                {
                    _accountUtil.RegisterNewUser(userName, email, password);
                    ClearRegisterInput();
                }
            }
        }

        private void ClearRegisterInput()
        {
            userName = string.Empty;
            email = string.Empty;
            password = string.Empty;
            repeatPassword = string.Empty;
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

        private void NavigateToProject(int projectId)
        {
            SessionService.Instance.ProjectId = projectId;
            Navigation.NavigateTo($"/mainMenu/{projectId}");
        }
    }
}
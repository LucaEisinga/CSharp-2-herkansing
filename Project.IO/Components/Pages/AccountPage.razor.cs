using BlazorBootstrap;
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
        private NavigationManager navigationManager { get; set; } = default!;

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
        }

        private void CreateNewUser()
        {
            if (IsEmpty(userName) || IsEmpty(email))
            {
                if (password.Equals(repeatPassword))
                {
                    _accountUtil.RegisterNewUser(userName, email, password);
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
            navigationManager.NavigateTo("/addProject");
        }

        private void NavigateToProject(int projectId)
        {
            SessionService.Instance.ProjectId = projectId;
            navigationManager.NavigateTo($"/mainMenu/{projectId}");
        }
    }
}
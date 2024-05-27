using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using Project.IO.Utilities;

namespace Project.IO.Components.Pages
{
    public partial class AccountPage
    {

        private Modal modal = default!;
        private DatabaseUtil databaseUtil = new DatabaseUtil();
        private AccountUtil accountUtil = new AccountUtil();
        private string? userName;
        private string? email;
        private string? password;
        private string? repeatPassword;

        private async Task OnShowModalClick()
        {
            await modal.ShowAsync();
        }

        private async Task OnHideModalClick()
        {
            await modal.HideAsync();
        }

        private void ShowConnection()
        {
            databaseUtil.CreateConnection();
        }

        private void CreateNewUser()
        {

            if (password.Equals(repeatPassword))
            {
                System.Diagnostics.Debug.WriteLine(userName);
                accountUtil.RegisterNewUser(userName, email, password, repeatPassword);
            }
        }

        private void NavigateToAddProject()
        {
            Navigation.NavigateTo("/addProject");
        }

    }
}

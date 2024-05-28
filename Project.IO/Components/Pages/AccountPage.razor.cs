using BlazorBootstrap;
using Project.IO.Utilities;

namespace Project.IO.Components.Pages
{
    public partial class AccountPage
    {

        private Modal modal = default!;
        private Modal errorModal = default!;
        private DatabaseUtil databaseUtil = new DatabaseUtil();
        private AccountUtil accountUtil = new AccountUtil();
        private string? logUser;
        private string? logPassword;
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

        private async Task LoginUser()
        {
            if (await accountUtil.canLogin(logUser, logPassword))
            {
                await modal.ShowAsync();
            }
        }

        private void CreateNewUser()
        {
            if (IsEmpty(userName) || IsEmpty(email))
            {
                if (password.Equals(repeatPassword))
                {
                    accountUtil.RegisterNewUser(userName, email, password, repeatPassword);
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

    }
}

using BlazorBootstrap;

namespace Project.IO.Components.Pages
{
    public partial class AccountPage
    {

        private Modal modal = default!;

        private async Task OnShowModalClick()
        {
            await modal.ShowAsync();
        }

        private async Task OnHideModalClick()
        {
            await modal.HideAsync();
        }

    }
}

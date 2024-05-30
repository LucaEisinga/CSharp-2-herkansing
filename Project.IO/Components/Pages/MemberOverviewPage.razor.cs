using BlazorBootstrap;

namespace Project.IO.Components.Pages
{
    public partial class MemberOverviewPage
    {

        private Modal memberModal = default!;

        private async Task ShowAddMemberModal()
        {
            await memberModal.ShowAsync();
        }

    }
}

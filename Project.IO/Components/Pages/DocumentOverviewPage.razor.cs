using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Project.IO.Utilities;
using Project.IO.Classes.Service;

namespace Project.IO.Components.Pages
{
    partial class DocumentOverviewPage
    {

        private FileUtil fileUtil = new FileUtil();

        [Inject]
        private NavigationManager Navigation { get; set; }

        private async Task FileUploading(InputFileChangeEventArgs e)
        {
            await fileUtil.HandleFileSelected(e);
        }

        private void NavigateToProject()
        {
            Navigation.NavigateTo($"/mainMenu/{SessionService.Instance.ProjectId}");
        }

    }
}

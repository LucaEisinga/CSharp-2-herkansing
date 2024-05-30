using BlazorBootstrap;
using System.Reflection;

namespace Project.IO.Components.Pages
{
    public partial class TaskPage
    {

        private Modal taskModal = default!;

        private async Task ShowTaskModal()
        {
            await taskModal.ShowAsync();
        }

    }
}

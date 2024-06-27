using Microsoft.AspNetCore.Components;
using Project.IO.Classes.Service;
using Project.IO.Utilities;

namespace Project.IO.Components.Layout
{
    partial class NavMenu
    {
        [Inject]
        NavigationManager NavigationManager { get; set; } = default!;
        [Inject]
        ProjectUtil projectUtil{get;set;} = default!;
        private void NavigateToProject()
        {
            var projectId = projectUtil.GetCurrentProject();
            SessionService.Instance.ProjectId = projectId.Id;
            NavigationManager.NavigateTo($"/mainMenu/{projectId.Id}");
        }
    }
}
using Microsoft.AspNetCore.Components;
using Project.IO.Classes;
using Project.IO.Classes.Service;
using Project.IO.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.IO.Components.Pages
{
    partial class MainMenuBase : LayoutComponentBase
    {
        [Inject]
        private ProjectUtil ProjectUtil { get; set; }

        [Inject]
        private NavigationManager Navigation { get; set; }

        [Parameter]
        public int ProjectId { get; set; }

        protected ProjectModel project;

        protected override async Task OnInitializedAsync()
        {
            project = await ProjectUtil.GetProjectById(ProjectId);
        }

        protected void Logout()
        {
            SessionService.Instance.Logout();
            Navigation.NavigateTo("/accountPage");
        }
    }
}

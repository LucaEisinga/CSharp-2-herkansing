using Project.IO.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.IO.Components.Pages
{
    public partial class AddProject
    {
        private ProjectUtil projectUtil;
        private string? projectTitle;
        private string? projectDescription;
        private DateTime projectDeadline;

        private async void CreateProject()
        {
            if (projectUtil != null)
            {
                await projectUtil.AddProjectToFirebaseAsync(projectTitle, projectDescription, projectDeadline);
            }
        }
    }
}

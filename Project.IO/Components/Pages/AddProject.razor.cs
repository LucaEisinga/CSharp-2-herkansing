using Project.IO.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.IO.Components.Pages
{
    public partial class AddProject
    {
        private ProjectUtil projectUtil = new ProjectUtil();
        private string? projectTitle;
        private string? projectDescription;
        private DateTime projectDeadline = DateTime.Now;

        private void CreateProject()
        {
            if (projectDeadline <  DateTime.Now.Date)
            {
                Debug.WriteLine("The project deadline cannot be in the past.");
                return;
            }
            projectUtil.AddProjectToFirebase(projectTitle, projectDescription, projectDeadline);
        }
    }
}

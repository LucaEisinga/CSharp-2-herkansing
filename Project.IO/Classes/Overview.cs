using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.IO.Classes
{
    internal class Overview
    {
        public List<Project> projects;

        public Overview()
        {
            projects = new List<Project>();
        }

        public void addProject(Project project)
        {
            projects.Add(project);
        }
    }
}

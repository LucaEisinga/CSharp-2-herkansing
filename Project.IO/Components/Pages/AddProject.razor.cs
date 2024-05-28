using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.IO.Components.Pages
{
    public partial class AddProject
    {
        private Project project = new Project();

        private void CreateProject()
        {
            // Logica om het project op te slaan of te verwerken
            Console.WriteLine($"Project aangemaakt: {project.Title}, {project.Description}, {project.Deadline}");
            // Hier kun je bijvoorbeeld een service aanroepen om het project op te slaan
        }

        public class Project
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public DateTime Deadline { get; set; } = DateTime.Today;
        }
    }
}

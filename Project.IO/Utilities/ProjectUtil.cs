using FireSharp.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.IO.Classes;
using FireSharp;
using FireSharp.Interfaces;
using System.Runtime.CompilerServices;
using System.Collections;
using Newtonsoft.Json;

namespace Project.IO.Utilities
{
    internal class ProjectUtil
    {
        private DatabaseUtil databaseUtil = new DatabaseUtil();

        public async Task<int> AutoIncrement()
        {
            FirebaseResponse response = await databaseUtil.CreateConnection().GetAsync("Project");
            string jsonResponse = response.Body;
            List<ProjectModel> projects = JsonConvert.DeserializeObject<List<ProjectModel>>(jsonResponse);

            int maxId = 0;

            if (projects.Count > 0)
            {
                foreach (var project in projects)
                {
                    if (project.Id > maxId)
                    {
                        maxId = project.Id;
                    }
                }
            }

            return maxId + 1;
        }

        public async System.Threading.Tasks.Task AddProjectToFirebaseAsync(string title, string description, DateTime deadline)
        {
            ProjectModel project = new ProjectModel(title, description, deadline);

            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }

            project.Id = await AutoIncrement();

            FirebaseClient client = databaseUtil.CreateConnection();

            SetResponse response = await client.SetAsync($"projects/{project.Id}", project);
            ProjectModel result = response.ResultAs<ProjectModel>();

            if (result != null)
            {
                Console.WriteLine($"Project {project.Title} added to Firebase succesfully.");
            }
            else
            {
                Console.WriteLine("Failed to add project to Firebase.");
            }
        }
    }
}

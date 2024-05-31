using FireSharp.Response;
using Project.IO.Classes;
using FireSharp;
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

            if (projects != null)
            {
                foreach (var project in projects)
                {
                    if (project != null && project.Id > maxId)
                    {
                        maxId = project.Id;
                    }
                }
            }

            return maxId + 1;
        }

        public async void AddProjectToFirebase(string title, string description, DateTime deadline)
        {
            
            int nextId = await AutoIncrement();
            ProjectModel project = new ProjectModel(title, description, deadline);
            project.Id = nextId;

            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }

            FirebaseClient client = databaseUtil.CreateConnection();

            SetResponse response = await client.SetAsync($"Project/{nextId}", project);
            /*ProjectModel result = response.ResultAs<ProjectModel>();

            if (result != null)
            {
                Console.WriteLine($"Project {project.Title} added to Firebase succesfully.");
            }
            else
            {
                Console.WriteLine("Failed to add project to Firebase.");
            }*/
        }
    }
}

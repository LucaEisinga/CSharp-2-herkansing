using FireSharp.Response;
using Project.IO.Classes;
using FireSharp;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Diagnostics;
using Project.IO.Classes.Model;
using Project.IO.Classes.Service;

namespace Project.IO.Utilities
{
    internal class ProjectUtil
    {
        private DatabaseUtil databaseUtil = new DatabaseUtil();

        public async Task<int> AutoIncrement()
        {
            int maxId = 0;

            try
            {
                FirebaseResponse response = await databaseUtil.CreateConnection().GetAsync("Project/");
                string jsonResponse = response.Body;
                List<ProjectModel> projects = JsonConvert.DeserializeObject<List<ProjectModel>>(jsonResponse);

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
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AutoIncrement: {ex.Message}");
            }

            return maxId + 1;
        }

        public async Task AddProjectToFirebase(string title, string description, DateTime deadline)
        {
            
            int nextId = await AutoIncrement();
            ProjectModel project = new ProjectModel(title, description, deadline);
            project.Id = nextId;
            SessionService.Instance.ProjectId = nextId;
            project.UserId = (int)SessionService.Instance.UserId;

            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }

            FirebaseClient client = databaseUtil.CreateConnection();

            SetResponse response = await client.SetAsync($"Project/{nextId}", project);
        }

        public async Task<List<ProjectModel>> GetListOfProjects()
        {
            try
            {
                FirebaseResponse response = await databaseUtil.CreateConnection().GetAsync("Project");
                string jsonResponse = response.Body;
                List<ProjectModel> projects = JsonConvert.DeserializeObject<List<ProjectModel>>(jsonResponse);

                if (projects == null)
                {
                    Debug.WriteLine("Deserialized projects list is null.");
                    return new List<ProjectModel>();
                }

                return projects;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception in GetListOfProjects: {ex.Message}");
                return new List<ProjectModel>();
            }
        }

        public async Task<List<ProjectModel>> GetProjectsForLoggedInUser()
        {
            if (!SessionService.Instance.IsLoggedIn)
                throw new InvalidOperationException("User isn't logged in");

            int? userId = SessionService.Instance.UserId;

            List<ProjectModel> projectModels = new List<ProjectModel>();

            FirebaseResponse response = await databaseUtil.CreateConnection().GetAsync("Project");
            string jsonResponse = response.Body;
            List<ProjectModel> projects = JsonConvert.DeserializeObject<List<ProjectModel>>(jsonResponse);

            if (projects != null)
            {
                foreach (var project in projects)
                {
                    if (project != null && project.UserId.Equals(userId))
                    {
                        ProjectModel instantProject = new ProjectModel(project.Title, project.Description, project.Deadline)
                        {
                            Id = project.Id,
                            UserId = project.UserId
                        };

                        projectModels.Add(instantProject);
                    }
                }
            }

            return projectModels;
        }

        public async Task<ProjectModel> GetProjectById(int projectId)
        {
            FirebaseResponse response = await databaseUtil.CreateConnection().GetAsync($"Project/{projectId}");
            return JsonConvert.DeserializeObject<ProjectModel>(response.Body);
        }
    }
}

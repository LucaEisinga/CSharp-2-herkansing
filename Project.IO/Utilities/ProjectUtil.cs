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
        private RoleService roleService = new RoleService();
        private AccountUtil accountUtil = new AccountUtil();
        private MemberProjectService memberProjectService = new MemberProjectService();
        private ProjectService projectService = new ProjectService();

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

            var currentUser = await accountUtil.GetCurrentLoggedInUserName();

            await projectService.AddNewParticipant(1);

            await roleService.AddRoleToMember(currentUser.username, "voorzitter");

            await memberProjectService.AddNewParticipant(currentUser.username, title);

            FirebaseClient client = databaseUtil.CreateConnection();

            SetResponse response = await client.SetAsync($"Project/{nextId}", project);
        }

        public async Task<ProjectModel> GetCurrentProject()
        {
            int? projectId = SessionService.Instance.ProjectId;

            FirebaseResponse response = await databaseUtil.CreateConnection().GetAsync($"Project/{projectId}");
            return JsonConvert.DeserializeObject<ProjectModel>(response.Body);
        }


        public async Task<List<Role>> GetProjectsForLoggedInUser()
        {
            if (!SessionService.Instance.IsLoggedIn)
                throw new InvalidOperationException("User isn't logged in");

            int? userId = SessionService.Instance.UserId;

            List<Role> memberProjectModels = new List<Role>();

            FirebaseResponse response = await databaseUtil.CreateConnection().GetAsync("Role/");
            string jsonResponse = response.Body;
            List<Role> projects = JsonConvert.DeserializeObject<List<Role>>(jsonResponse);

            if (projects != null)
            {
                foreach (var project in projects)
                {
                    if (project != null && project.UserId.Equals(userId))
                    {
                        memberProjectModels.Add(project);
                    }
                }
            }

            return memberProjectModels;
        }

        public async Task<ProjectModel> GetProjectById(int projectId)
        {
            FirebaseResponse response = await databaseUtil.CreateConnection().GetAsync($"Project/{projectId}");
            return JsonConvert.DeserializeObject<ProjectModel>(response.Body);
        }
    }
}

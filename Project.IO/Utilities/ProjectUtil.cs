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


        public async Task<List<MemberProjectModel>> GetProjectsForLoggedInUser()
        {
            if (!SessionService.Instance.IsLoggedIn)
                throw new InvalidOperationException("User isn't logged in");

            int? userId = SessionService.Instance.UserId;

            List<MemberProjectModel> memberProjectModels = new List<MemberProjectModel>();

            FirebaseResponse response = await databaseUtil.CreateConnection().GetAsync("MemberProject/");
            string jsonResponse = response.Body;
            List<MemberProjectModel> projects = JsonConvert.DeserializeObject<List<MemberProjectModel>>(jsonResponse);

            if (projects != null)
            {
                foreach (var project in projects)
                {
                    if (project != null && project.UserId.Equals(userId))
                    {
                        MemberProjectModel instantProject = new MemberProjectModel(project.Username, project.ProjectName)
                        {
                            Id = project.Id,
                            UserId = project.UserId,
                            ProjectId = project.ProjectId
                        };

                        memberProjectModels.Add(instantProject);
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

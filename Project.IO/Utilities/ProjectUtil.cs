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
        private ProjectAssignmentService projectService = new ProjectAssignmentService();

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

        public async Task createProject(string title, string description, DateTime deadline)
        {
            
            int nextId = await AutoIncrement();
            ProjectModel project = new ProjectModel(title, description, deadline);
            project.Id = nextId;
            await roleService.initializeDefaultRoles(nextId);
            SessionService.Instance.ProjectId = nextId;
            project.UserId = (int)SessionService.Instance.UserId;

            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }

            await projectService.AddNewParticipant(1);

            FirebaseClient client = databaseUtil.CreateConnection();

            SetResponse response = await client.SetAsync($"Project/{nextId}", project);
        }

        public async Task<ProjectModel> GetCurrentProject()
        {
            int? projectId = SessionService.Instance.ProjectId;

            FirebaseResponse response = await databaseUtil.CreateConnection().GetAsync($"Project/{projectId}");
            return JsonConvert.DeserializeObject<ProjectModel>(response.Body);
        }


        public async Task<List<ProjectModel>> GetProjectsForLoggedInUser()
        {
            if (!SessionService.Instance.IsLoggedIn)
                throw new InvalidOperationException("User isn't logged in");

            int? userId = SessionService.Instance.UserId;

            List<ProjectModel> projects = new List<ProjectModel>();

            FirebaseResponse response = await databaseUtil.CreateConnection().GetAsync($"ProjectAssignment/UserId/{userId}");
            string jsonResponse = response.Body;
            List<ProjectAssignment> projectAssignments = JsonConvert.DeserializeObject<List<ProjectAssignment>>(jsonResponse);

            if (projectAssignments != null)
            {
                foreach (var projectAssignment in projectAssignments)
                {
                    if (projectAssignment != null && projectAssignment.UserId.Equals(userId))
                    {
                        FirebaseResponse tempResponse = await databaseUtil.CreateConnection().GetAsync($"Project/Id/{projectAssignment.ProjectId}");
                        ProjectModel project = JsonConvert.DeserializeObject<ProjectModel>(tempResponse.Body);
                        projects.Add(project);
                    }
                }
            }

            return projects;
        }

        public async Task<ProjectModel> GetProjectById(int projectId)
        {
            FirebaseResponse response = await databaseUtil.CreateConnection().GetAsync($"Project/{projectId}");
            return JsonConvert.DeserializeObject<ProjectModel>(response.Body);
        }

        public async Task<List<Member>> GetMembersInProject()
        {
            List<ProjectAssignment> projectAssignments = await projectService.GetProjectAssignments();
            List<Member> projectMembers = new List<Member>();
            foreach(ProjectAssignment assignment in projectAssignments)
            {
                FirebaseResponse response = await databaseUtil.CreateConnection().GetAsync($"Member/{assignment.UserId}");
                Member member = JsonConvert.DeserializeObject<Member>(response.Body);
                projectMembers.Add(member);
            }
            return projectMembers;
        }
    }
}

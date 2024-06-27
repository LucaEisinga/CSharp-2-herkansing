using FireSharp.Response;
using Newtonsoft.Json;
using Project.IO.Classes.Model;
using Project.IO.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.IO.Classes.Service
{
    internal class ProjectAssignmentService
    {

        private DatabaseUtil databaseUtil = new DatabaseUtil();

        public async Task<int> AutoIncrementId()
        {
            FirebaseResponse response = await databaseUtil.CreateConnection().GetAsync("ProjectAssignment");
            string jsonResponse = response.Body;
            List<ProjectAssignment> participants = JsonConvert.DeserializeObject<List<ProjectAssignment>>(jsonResponse);

            int maxId = 0;

            if (participants != null)
            {
                foreach (ProjectAssignment participant in participants)
                {
                    if (participant != null && participant.Id > maxId)
                    {
                        maxId = participant.Id;
                    }
                }
            }

            return maxId + 1;
        }

        public async Task AddNewParticipant(int RoleId)
        {
            int UserId = (int)SessionService.Instance.UserId;
            await AddNewParticipant(UserId, RoleId);
        }

        public async Task AddNewParticipant(int userId, int RoleId)
        {
            int nextId = await AutoIncrementId();
            int ProjectId = (int)SessionService.Instance.ProjectId;
            ProjectAssignment participant = new ProjectAssignment(userId, ProjectId, RoleId)
            {
                Id = nextId
            };
            await databaseUtil.CreateConnection().SetAsync($"ProjectAssignment/{nextId}", participant);
        }
        public async Task<List<ProjectAssignment>> GetProjectAssignments()
        {
            int ProjectId = (int)SessionService.Instance.ProjectId;
            FirebaseResponse response = await databaseUtil.CreateConnection().GetAsync($"ProjectAssignment");
            List<ProjectAssignment> allAssignments = JsonConvert.DeserializeObject<List<ProjectAssignment>>(response.Body);
            List< ProjectAssignment> projectAssignments = [];
            if (allAssignments != null)
            {
                foreach (var projectAssignment in allAssignments)
                {
                    if (projectAssignment != null && projectAssignment.ProjectId.Equals(ProjectId))
                    {
                        projectAssignments.Add(projectAssignment);
                    }
                }
            }
            return projectAssignments;
        }
        public async Task<ProjectAssignment> getMemberData(int id)
        {
            FirebaseResponse response = await databaseUtil.CreateConnection().GetAsync($"ProjectAssignment/{id}");
            return JsonConvert.DeserializeObject<ProjectAssignment>(response.Body);
        }
    }
}

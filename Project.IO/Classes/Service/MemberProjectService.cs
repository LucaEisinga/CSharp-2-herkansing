using FireSharp.Response;
using Newtonsoft.Json;
using Project.IO.Classes.Model;
using Project.IO.Utilities;

namespace Project.IO.Classes.Service
{
    internal class MemberProjectService
    {

        private DatabaseUtil databaseUtil = new DatabaseUtil();

        public async Task<int> AutoIncrementMemberProject()
        {
            FirebaseResponse response = await databaseUtil.CreateConnection().GetAsync("MemberProject");
            string jsonResponse = response.Body;
            List<MemberProjectModel> participants = JsonConvert.DeserializeObject<List<MemberProjectModel>>(jsonResponse);

            int maxId = 0;

            if (participants != null)
            {
                foreach (MemberProjectModel participant in participants)
                {
                    if (participant != null && participant.Id > maxId)
                    {
                        maxId = participant.Id;
                    }
                }
            }

            return maxId + 1;
        }

        public async Task AddNewParticipant(string username, string projectName)
        {
            int nextId = await AutoIncrementMemberProject();
            MemberProjectModel participant = new MemberProjectModel(username, projectName)
            {
                Id = nextId,
                UserId = (int)SessionService.Instance.UserId,
                ProjectId = (int)SessionService.Instance.ProjectId
            };

            await databaseUtil.CreateConnection().SetAsync($"MemberProject/{nextId}", participant);
        }

        public async Task AddNewParticipantChosenMember(int userId, string username, string projectName)
        {
            int nextId = await AutoIncrementMemberProject();
            MemberProjectModel participant = new MemberProjectModel(username, projectName)
            {
                Id = nextId,
                UserId = userId,
                ProjectId = (int)SessionService.Instance.ProjectId
            };

            await databaseUtil.CreateConnection().SetAsync($"MemberProject/{nextId}", participant);
        }
    }
}
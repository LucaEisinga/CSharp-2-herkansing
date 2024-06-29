using FireSharp.Response;
using Newtonsoft.Json;
using Project.IO.Classes.Model;
using Project.IO.Utilities;

namespace Project.IO.Classes.Service
{
    internal class DeadlineService
    {

        private DatabaseUtil databaseUtil = new DatabaseUtil();

        public async Task<int> AutoIncrementDeadline()
        {
            FirebaseResponse response = await databaseUtil.CreateConnection().GetAsync("Deadline/");
            string jsonResponse = response.Body;
            List<DeadlineModel> deadlines = JsonConvert.DeserializeObject<List<DeadlineModel>>(jsonResponse);

            int maxId = 0;

            if (deadlines != null)
            {
                foreach (DeadlineModel deadline in deadlines)
                {
                    if (deadline != null && deadline.Id > maxId)
                    {
                        maxId = deadline.Id;
                    }
                }

            }

            return maxId + 1;
        }

        public async Task AddNewDeadline(int userId, string username, string taskName, DateTime taskDeadline)
        {

            int nextId = await AutoIncrementDeadline();

            DeadlineModel deadline = new DeadlineModel(userId, username, taskName, taskDeadline);
            deadline.Id = nextId;
            deadline.ProjectId = SessionService.Instance.ProjectId;

            SetResponse response = await databaseUtil.CreateConnection().SetAsync($"Deadline/{nextId}", deadline);
        }

        public async Task<List<DeadlineModel>> GetAllDeadlines()
        {

            FirebaseResponse response = await databaseUtil.CreateConnection().GetAsync("Deadline/");
            string jsonResponse = response.Body;
            List<DeadlineModel> deadlines = JsonConvert.DeserializeObject<List<DeadlineModel>>(jsonResponse);

            List<DeadlineModel> deadlineList = new List<DeadlineModel>();
            int? projectId = SessionService.Instance.ProjectId;

            if (deadlines != null)
            {
                foreach (DeadlineModel deadline in deadlines)
                {
                    if (deadline != null && deadline.ProjectId.Equals(projectId))
                    {
                        DeadlineModel currentDeadline = new DeadlineModel(deadline.UserId, deadline.Username, deadline.TaskName, deadline.Deadline)
                        {
                            Id = deadline.Id,
                            ProjectId = deadline.ProjectId
                        };

                        deadlineList.Add(currentDeadline);
                    }
                }
            }

            return deadlineList;
        }

        public async Task DeleteDeadline(int id)
        {
            await databaseUtil.CreateConnection().DeleteAsync($"Deadline/{id}");
        }

    }
}

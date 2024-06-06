
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

            SetResponse response = await databaseUtil.CreateConnection().SetAsync($"Deadline/{nextId}", deadline);

        }

        public async Task<List<DeadlineModel>> GetAllDeadlines()
        {

            FirebaseResponse response = await databaseUtil.CreateConnection().GetAsync("Deadline/");
            string jsonResponse = response.Body;
            List<DeadlineModel> deadlines = JsonConvert.DeserializeObject<List<DeadlineModel>>(jsonResponse);

            return deadlines;

        }

    }
}

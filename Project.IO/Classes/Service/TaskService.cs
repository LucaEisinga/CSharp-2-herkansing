using FireSharp.Response;
using Project.IO.Utilities;
using Project.IO.Classes.Model;
using Newtonsoft.Json;

namespace Project.IO.Classes.Service
{
    internal class TaskService
    {

        private DatabaseUtil databaseUtil = new DatabaseUtil();
        
        public async Task<int> AutoIncrementTask()
        {

            FirebaseResponse response = await databaseUtil.CreateConnection().GetAsync("Task/");
            string jsonResponse = response.Body;
            List<TaskModel> tasks = JsonConvert.DeserializeObject<List<TaskModel>>(jsonResponse);

            int maxId = 0;

            if (tasks != null)
            {
                foreach (TaskModel task in tasks)
                {
                    if (task != null && task.Id > maxId)
                    {
                        maxId = task.Id;
                    }
                }
            }

            return maxId + 1;
        }

        public async Task AddNewTaskToProject(string taskTitle, int projectMember, string taskDescription, DateTime taskDeadline)
        {

            int nextId = await AutoIncrementTask();

            TaskModel task = new TaskModel(taskTitle, taskDescription, taskDeadline);
            task.Id = nextId;
            task.UserId = projectMember;

            SetResponse response = await databaseUtil.CreateConnection().SetAsync($"Task/{nextId}", task);

        }

        public async Task<List<Member>> GetAllMembersInProject()
        {

            FirebaseResponse response = await databaseUtil.CreateConnection().GetAsync("Member/");
            string jsonResponse = response.Body;
            List<Member> members = JsonConvert.DeserializeObject<List<Member>>(jsonResponse);

            return members;
        }


    }
}
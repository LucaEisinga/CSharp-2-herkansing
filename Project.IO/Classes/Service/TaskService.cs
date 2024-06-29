using FireSharp.Response;
using Project.IO.Utilities;
using Project.IO.Classes.Model;
using Newtonsoft.Json;

namespace Project.IO.Classes.Service
{
    public class TaskService
    {

        private DatabaseUtil databaseUtil = new DatabaseUtil();
        private DeadlineService deadlineService = new DeadlineService();
        
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

            TaskModel task = new TaskModel(taskTitle, taskDescription, taskDeadline)
            {
                Id = nextId,
                UserId = projectMember,
                ProjectId = SessionService.Instance.ProjectId
            };

            await deadlineService.AddNewDeadline(projectMember, await this.GetMemberNameUsingId(projectMember), taskTitle, taskDeadline);

            SetResponse response = await databaseUtil.CreateConnection().SetAsync($"Task/{nextId}", task);
        }

        public async Task<List<TaskModel>> GetAllTasks()
        {
            FirebaseResponse response = await databaseUtil.CreateConnection().GetAsync("Task/");
            string jsonResponse = response.Body;

            // Deserialize and filter out null or incomplete tasks
            var tasks = JsonConvert.DeserializeObject<List<TaskModel>>(jsonResponse) ?? new List<TaskModel>();

            var validTasks = tasks
                .Where(task => task != null && !string.IsNullOrWhiteSpace(task.Title) && task.Id > 0 && task.ProjectId.Equals(SessionService.Instance.ProjectId))
                .ToList();

            // Debug logging to check valid tasks count
            Console.WriteLine($"Retrieved {validTasks.Count} valid tasks");

            return validTasks;
        }

        public async Task<string> GetMemberNameUsingId(int userId)
        {

            FirebaseResponse response = await databaseUtil.CreateConnection().GetAsync("Member/");
            string jsonResponse = response.Body;
            List<Member> members = JsonConvert.DeserializeObject<List<Member>>(jsonResponse);

            string? nameOfMember = null;

            if (members != null)
            {
                foreach (Member member in members)
                {
                    if (member != null && member.Id.Equals(userId))
                    {
                        nameOfMember = member.username;
                    }
                }
            }

            return nameOfMember;
        }
        public async Task<TaskModel?> GetTaskById(int id)
        {
            FirebaseResponse response = await databaseUtil.CreateConnection().GetAsync($"Task/{id}");
            string jsonResponse = response.Body;
            return JsonConvert.DeserializeObject<TaskModel>(jsonResponse);
        }
        public async Task DeleteTask(int id)
        {
            await databaseUtil.CreateConnection().DeleteAsync($"Task/{id}");
        }
    }
}

namespace Project.IO.Classes.Model
{
    internal class DeadlineModel
    {

        public int Id { get; set; }
        public int UserId { get; set; }
        public string? Username { get; set; }
        public string? TaskName { get; set; }
        public DateTime Deadline { get; set; }

        public DeadlineModel(int UserId, string? Username, string? TaskName, DateTime Deadline) 
        {
            this.UserId = UserId;
            this.Username = Username;
            this.TaskName = TaskName;
            this.Deadline = Deadline;
        }

    }
}

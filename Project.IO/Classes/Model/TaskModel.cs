
namespace Project.IO.Classes.Model
{
    public class TaskModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int UserId { get; set; }
        public DateTime Deadline { get; set; }
        public string Description { get; set; }

        public TaskModel(string title, string description, DateTime deadline)
        {
            Title = title;
            Description = description;
            Deadline = deadline;
        }
    }
}

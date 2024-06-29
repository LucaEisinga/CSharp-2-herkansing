namespace Project.IO.Classes
{
    public class ProjectModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
        public int UserId { get; set; }

        public ProjectModel(string title, string description, DateTime deadline)
        {
            Title = title;
            Description = description;
            Deadline = deadline;
        }
    }
}
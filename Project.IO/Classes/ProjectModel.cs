using System;
using System.Collections.Generic;

namespace Project.IO.Classes
{
    internal class ProjectModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; }

        public ProjectModel(string title, string description, DateTime deadline)
        {
            Title = title;
            Description = description;
            Deadline = deadline;
        }

        /*public void AddTask(Task task)
        {
            if (task == null)
            {
                throw new ArgumentNullException(nameof(task));
            }

            Tasks.Add(task);
        }

        public void AddMemberToProject(Member member)
        {
            if (member == null)
            {
                throw new ArgumentNullException(nameof(member));
            }

            Members.Add(member);
        }*/
    }
}

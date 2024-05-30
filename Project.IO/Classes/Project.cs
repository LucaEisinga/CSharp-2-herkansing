using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.IO.Classes.Model;

namespace Project.IO.Classes
{
    internal class Project
    {
        public int Id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public DatePicker deadline { get; set; }
        public List<Task> tasks { get; set; }
        public List<Member> members { get; set; }

        public Project(string title, string description) 
        { 
            this.title = title;
            this.description = description;
            this.deadline = new DatePicker();
            this.tasks = new List<Task>();
            this.members = new List<Member>();
        }

        public void addTask(Task task)
        {
            tasks.Add(task);
        }

        public void addMemberToProject(Member member)
        {
            members.Add(member);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.IO.Classes
{
    internal class Task
    {
        public int Id { get; set; }
        public string title { get; set; }
        public List<Member> members { get; set; }
        public DatePicker deadline { get; set; }
        public string description { get; set; }
        public int difficulty { get; set; }

        public Task(string title, string description, int difficulty)
        {
            this.title = title;
            this.description = description;
            this.difficulty = difficulty;
            this.members = new List<Member>();
            deadline = new DatePicker();
        }
        
        public void addMemberToTask(Member member)
        {
            members.Add(member);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.IO.Classes.Model;

namespace Project.IO.Classes
{
    internal class Task
    {
        public int Id { get; set; }
        public string title { get; set; }
        public DatePicker deadline { get; set; }
        public string description { get; set; }
        public int difficulty { get; set; }

        public Task(string title, string description, int difficulty)
        {
            this.title = title;
            this.description = description;
            this.difficulty = difficulty;
            deadline = new DatePicker();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.IO.Classes.Models
{
    public class TaskModel
    {
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string AssignedTo { get; set; }
        public string Description { get; set; }
    }
}

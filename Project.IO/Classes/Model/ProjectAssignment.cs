using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.IO.Classes.Model
{
    public class ProjectAssignment
    {

        public int Id { get; set; }
        public int UserId { get; set; }
        public int? ProjectId { get; set; }
        public int RoleId { get; set; }

        public ProjectAssignment(int user, int project, int role)
        {
            UserId = user;
            ProjectId = project;
            RoleId = role;
        }
    }
}

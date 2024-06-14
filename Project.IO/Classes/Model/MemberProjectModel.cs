using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.IO.Classes.Model
{
    internal class MemberProjectModel
    {

        public int Id { get; set; }
        public int UserId { get; set; }
        public string? Username {  get; set; }
        public int ProjectId { get; set; }
        public string? ProjectName { get; set; }

        public MemberProjectModel(string Username, string ProjectName) 
        {
            this.Username = Username;
            this.ProjectName = ProjectName;
        }

    }
}

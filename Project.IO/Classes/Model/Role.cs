namespace Project.IO.Classes.Model
{
    public class Role
    {

        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string RoleName { get; set; }

        public Role(string RoleName, int ProjectId)
        {
            this.setRoleName(RoleName);
            this.ProjectId = ProjectId;
        }
        private string getRoleName()
        {
            return this.RoleName;
        }

        private void setRoleName(string RoleName)
        {
            if (RoleName != null)
            {
                this.RoleName = RoleName;
            }
        }


    }
}

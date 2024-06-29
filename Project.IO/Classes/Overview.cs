namespace Project.IO.Classes
{
    internal class Overview
    {
        public List<ProjectModel> projects;

        public Overview()
        {
            projects = new List<ProjectModel>();
        }

        public void addProject(ProjectModel project)
        {
            projects.Add(project);
        }
    }
}

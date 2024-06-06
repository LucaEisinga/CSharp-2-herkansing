using BlazorBootstrap;
using Project.IO.Classes.Model;
using System.Reflection;
using System.Diagnostics;
using Project.IO.Classes.Service;

namespace Project.IO.Components.Pages
{
    public partial class TaskPage
    {

        private Modal taskModal = default!;
        private TaskService taskService = new TaskService();
        private string? taskTitle;
        private int? projectMember;
        private DateTime taskDeadline = DateTime.Now;
        private string? taskDescription;

        private List<Member> members = new List<Member>();

        private async Task ShowTaskModal()
        {

            try
            {
                members = await ShowAllMembers();
                if (members == null)
                {
                    Debug.WriteLine("Member list is null after fetching.");
                }
                else
                {
                    Debug.WriteLine($"Fetched {members.Count} members.");
                }

                await taskModal.ShowAsync();
            } 
            catch (Exception ex)
            {
                Debug.WriteLine($"Error fetching members: {ex.Message}");
            }
        }

        private async Task<List<Member>> ShowAllMembers()
        {
            var memberList = await taskService.GetAllMembersInProject();

            if (memberList == null)
            {
                Debug.WriteLine("Member list is null in ShowAllMembers method.");
            }
            else
            {
                Debug.WriteLine($"ShowAllMembers fetched {memberList.Count} members.");
            }

            return memberList;
        }

        private async void AddNewTaskToUser()
        {
            if (IsEmpty(taskTitle) || IsEmpty(taskDescription))
            {
                if (projectMember != null)
                {
                    await taskService.AddNewTaskToProject(taskTitle, projectMember.Value, taskDescription, taskDeadline);
                }
            }
        }

        private bool IsEmpty(string value)
        {
            bool result = false;

            if (!(value == null) || !(value == ""))
            {
                result = true;
            }

            return result;
        }

    }
}

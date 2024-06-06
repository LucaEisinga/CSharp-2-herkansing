using BlazorBootstrap;
using Project.IO.Classes.Model;
using System.Reflection;
using System.Diagnostics;
using Project.IO.Classes.Service;
using Microsoft.AspNetCore.Components;

namespace Project.IO.Components.Pages
{
    public partial class TaskPage
    {

        private Modal taskModal = default!;
        private string? taskTitle;
        private int? projectMember;
        private DateTime taskDeadline = DateTime.Now;
        private string? taskDescription;

        private List<Member> members = new List<Member>();
        private List<TaskModel> tasks = new List<TaskModel>();

        [Inject]
        public TaskService TaskService { get; set; } = default!;

        [Inject]
        private NavigationManager navigationManager { get; set; } = default!;
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

        public async Task<List<Member>> ShowAllMembers()
        {
            var memberList = await TaskService.GetAllMembersInProject();

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
                    await TaskService.AddNewTaskToProject(taskTitle, projectMember.Value, taskDescription, taskDeadline);
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
        private void NavigateToTask(int taskId)
        {
            navigationManager.NavigateTo($"/taskView/{taskId}");
        }
    }
}

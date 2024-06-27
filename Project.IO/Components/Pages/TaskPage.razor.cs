using BlazorBootstrap;
using Project.IO.Classes.Model;
using System.Diagnostics;
using Project.IO.Classes.Service;
using Project.IO.Utilities;
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

        private List<Member> members = [];
        private List<TaskModel> tasks = [];
        [Inject]
        private AccountUtil accountUtil { get; set; } = default!;
        [Inject]
        private ProjectUtil projectUtil { get; set; } = default!;

        [Inject]
        private TaskService TaskService { get; set; } = default!;
        [Inject]
        private ProjectAssignmentService projectService { get; set; } = default!;

        [Inject]
        private NavigationManager navigationManager { get; set; } = default!;
        protected override async Task OnInitializedAsync()
        {   
            try
            {
                // Fetch the task data
                await getTasks();

                // Debug logging
                Console.WriteLine($"Loaded {tasks.Count} tasks on initialization");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading tasks: {ex.Message}");
            }
        }
        public async Task ShowTaskModal()
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
            var memberList = await projectUtil.GetMembersInProject();

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
        private async Task<List<TaskModel>> getTasks()
        {
            tasks = await TaskService.GetAllTasks();

            if (tasks == null)
            {
                Debug.WriteLine("Task list is null in ShowAllMembers method.");
            }
            else
            {
                Debug.WriteLine($"GetTasks fetched {tasks.Count} tasks.");
            }

            return tasks;
        }

        private async Task AddNewTaskToUser()
        {
            if (IsEmpty(taskTitle) || IsEmpty(taskDescription))
            {
                if (projectMember != null)
                {
                    await TaskService.AddNewTaskToProject(taskTitle, projectMember.Value, taskDescription, taskDeadline);
                    await getTasks();
                    ClearInputMenu();
                    await taskModal.HideAsync();
                }
            }
        }
        private void ClearInputMenu()
        {
            taskTitle = string.Empty;
            projectMember = null;
            taskDeadline = DateTime.Now;
            taskDescription = string.Empty;
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
        public void NavigateToTask(int taskId)
        {
            Console.WriteLine(taskId);
            navigationManager.NavigateTo($"/taskView/{taskId}");
        }
        private string GetButtonColour(DateTime deadline)
        {
            var date = DateTime.Today;
            var difference = (deadline - date).TotalDays;
            switch(difference)
            {
                case < 0:
                    return "btn long-button-red";
                case <= 7:
                    return "btn long-button-yellow";
                default:
                    return "btn long-button-green";
            }
        }
    }
}

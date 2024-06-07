﻿using Microsoft.AspNetCore.Components;
using Project.IO.Classes.Model;
using Project.IO.Classes.Service;

namespace Project.IO.Components.Pages
{
    public partial class TaskView : ComponentBase
	{
		[Inject]
		protected TaskService TaskService { get; set; }
		[Inject]
		private NavigationManager navigationManager { get; set; } = default!;
		[Parameter]
        public int TaskId { get; set;}
		private string name;

		protected TaskModel Task { get; set; }

		protected override async Task OnInitializedAsync()
		{
			Console.WriteLine(TaskId);
			// Fetch the task data
			Task = await TaskService.GetTaskById(TaskId);
			name = await getMemberName();
		}
		private async Task<string> getMemberName()
		{
			return await TaskService.GetMemberNameUsingId(Task.UserId);
		}
		private async Task DeleteTask()
		{
			await TaskService.DeleteTask(TaskId);
			navigationManager.NavigateTo("/taskPage");
		}
	}
}

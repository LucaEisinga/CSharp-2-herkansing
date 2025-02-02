﻿using Microsoft.AspNetCore.Components;
using Project.IO.Classes.Model;
using Project.IO.Classes.Service;
using Project.IO.Utilities;

namespace Project.IO.Components.Pages
{
    public partial class TaskView : ComponentBase
	{
		[Inject]
		protected TaskService TaskService { get; set; }
		[Inject]
		protected AccountUtil accountUtil { get; set; }
		[Inject]
		private NavigationManager navigationManager { get; set; } = default!;
		[Parameter]
        public int TaskId { get; set;}
		private Member TaskOwner;

		protected TaskModel Task;

		protected override async Task OnInitializedAsync()
		{
			Console.WriteLine(TaskId);
			// Fetch the task data
			Task = await TaskService.GetTaskById(TaskId);
			TaskOwner = await getMemberName();
		}
		private async Task<Member> getMemberName()
		{
			return await accountUtil.GetMemberById(Task.UserId);
		}
		private async Task DeleteTask()
		{
			await TaskService.DeleteTask(TaskId);
			navigationManager.NavigateTo("/taskPage");
		}
	}
}

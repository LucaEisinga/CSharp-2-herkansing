using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using Project.IO.Classes.Models;
using Project.IO.Classes.Services;

namespace Project.IO.Components.Pages
{
    public partial class TaskView : ComponentBase
	{
		[Inject]
		protected TaskService TaskService { get; set; }

		protected TaskModel Task { get; set; }

		protected override void OnInitialized()
		{
			// Fetch the task data
			Task = TaskService.GetTask();
		}
	}
}

using Microsoft.AspNetCore.Components;
using Project.IO.Classes.Model;
using Project.IO.Classes.Service;

namespace Project.IO.Components.Pages
{
    public partial class TaskView : ComponentBase
	{
		[Inject]
		protected TaskService TaskService { get; set; }
		[Parameter]
        public int TaskId { get; set;}

		protected TaskModel Task { get; set; }

		protected override async Task OnInitializedAsync()
		{
			// Fetch the task data
			Task = await TaskService.GetTask(TaskId);

		}
	}
}

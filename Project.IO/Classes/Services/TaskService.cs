using Project.IO.Classes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.IO.Classes.Services
{
	public class TaskService
	{
		public TaskModel GetTask()
		{
			// Simulating fetching data. Replace this with actual Firebase data retrieval.
			return new TaskModel
			{
				Title = "Sample Task",
				Date = DateTime.Now,
				AssignedTo = "John Doe",
				Description = "This is a sample task description."
			};
		}
	}
}

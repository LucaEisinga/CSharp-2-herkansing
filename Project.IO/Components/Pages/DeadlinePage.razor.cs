
using Project.IO.Classes.Model;
using Project.IO.Classes.Service;

namespace Project.IO.Components.Pages
{
    partial class DeadlinePage
    {

        private DeadlineService deadlineService = new DeadlineService();
        private List<DeadlineModel> deadlines;

        protected override async Task OnInitializedAsync()
        {
            deadlines = await deadlineService.GetAllDeadlines();
        }

    }
}

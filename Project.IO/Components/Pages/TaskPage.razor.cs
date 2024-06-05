using BlazorBootstrap;
using Project.IO.Classes.Model;
using System.Reflection;
using System.Diagnostics;

namespace Project.IO.Components.Pages
{
    public partial class TaskPage
    {

        private Modal taskModal = default!;
        private string? taskTitle;
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
            var memberList = new List<Member>();

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

    }
}

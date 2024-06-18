using Project.IO.Classes.Service;
using Project.IO.Classes.Model;
using Microsoft.AspNetCore.Components;
using System.Diagnostics;

namespace Project.IO.Components.Pages
{
    partial class RoleChangePage
    {
        [Parameter]
        public int RoleId { get; set; }
        protected RoleService? roleService = new RoleService();

        [Inject]
        public NavigationManager? Navigation { get; set; }

        private string? newestRole;
        protected Role? CurrentRole { get; set; }
        private string? chosenMember;

        protected override async Task OnInitializedAsync()
        {
            System.Diagnostics.Debug.WriteLine(RoleId);
            // Fetch the role data
            CurrentRole = await roleService.GetRoleById(RoleId);
            chosenMember = await GetMemberFromRole();
        }
        private async Task<string?> GetMemberFromRole()
        {
            return await roleService.GetRoleMemberByUserId(CurrentRole.UserId);
        }
        private async Task UpdateRole()
        {
            await roleService.UpdateRoleOfUser(RoleId, newestRole);
        }
        public void NavigateBackToMemberOverview()
        {
            Navigation.NavigateTo("/memberOverviewPage");
        }
    }
}

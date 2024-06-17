using Project.IO.Classes.Service;
using Project.IO.Classes.Model;
using Microsoft.AspNetCore.Components;

namespace Project.IO.Components.Pages
{
    partial class RoleChangePage
    {
        private RoleService roleService = new RoleService();
        private string newestRole;
        public int RoleId { get; set; }
        protected Role CurrentRole { get; set; }
        private Member chosenMember;
        [Inject]
        private NavigationManager navigationManager { get; set; } = default!;
        protected override async Task OnInitializedAsync()
        {
            Console.WriteLine(RoleId);
            // Fetch the role data
            CurrentRole = await roleService.GetRoleById(RoleId);
            chosenMember = await GetMemberFromRole();
        }
        private async Task<Member> GetMemberFromRole()
        {
            return await roleService.GetRoleMemberByUserId(CurrentRole.UserId);
        }
        private async Task UpdateRole()
        {
            await roleService.UpdateRoleOfUser(RoleId, newestRole);
        }
        public void NavigateBackToMemberOverview()
        {
            navigationManager.NavigateTo("/memberOverviewPage");
        }
    }
}

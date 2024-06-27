using Project.IO.Classes.Service;
using Project.IO.Classes.Model;
using Project.IO.Utilities;
using Microsoft.AspNetCore.Components;
using System.Diagnostics;

namespace Project.IO.Components.Pages
{
    partial class EditMemberRolePage
    {
        [Parameter]
        public int Id { get; set; }
        [Inject]
        private AccountUtil accountUtil { get; set; } = default!;
        [Inject]
        private ProjectAssignmentService projectService{get;set;} = default!;
        [Inject]
        private RoleService? roleService { get; set; } = default!;

        [Inject]
        public NavigationManager? Navigation { get; set; }

        private string? newestRole;
        protected ProjectAssignment? memberData { get; set; }
        private Member? chosenMember;
        private List<Role> roles = new List<Role>();

        protected override async Task OnInitializedAsync()
        {
            System.Diagnostics.Debug.WriteLine(Id);
            // Fetch the role data
            memberData = await projectService.getMemberData(Id);
            chosenMember = await accountUtil.GetMemberById(memberData.UserId);
            roles = await roleService.getRolesForProject();
        }
        private async Task UpdateRole()
        {
            await roleService.UpdateRoleOfUser(Id, newestRole);
        }
        public void NavigateBackToMemberOverview()
        {
            Navigation.NavigateTo("/memberOverviewPage");
        }
    }
}

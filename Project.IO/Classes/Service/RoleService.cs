using Project.IO.Utilities;
using Project.IO.Classes.Model;
using FireSharp.Response;
using Newtonsoft.Json;

namespace Project.IO.Classes.Service
{
    public class RoleService
    {

        private DatabaseUtil databaseUtil = new DatabaseUtil();
        private MemberProjectService memberProjectService = new MemberProjectService();

        private async Task<int> AutoIncrementRole()
        {
            FirebaseResponse response = await databaseUtil.CreateConnection().GetAsync("Role");
            string jsonResponse = response.Body;
            List<Role> roles = JsonConvert.DeserializeObject<List<Role>>(jsonResponse);

            int maxId = 0;

            if (roles != null)
            {
                foreach (var role in roles)
                {
                    if (role != null && role.Id > maxId)
                    {
                        maxId = role.Id;
                    }
                }
            }

            return maxId + 1;
        }

        public async Task AddRoleToMember(string username, string roleName)
        {

            if (await hasRoleAlready(await getUserIdChosenForRole(username)))
            {
                int nextId = await AutoIncrementRole();
                int chosenUserId = await getUserIdChosenForRole(username);

                Role role = new Role(roleName);
                role.Id = nextId;
                role.UserId = chosenUserId;
                role.ProjectId = SessionService.Instance.ProjectId;
                role.Username = username;

                var projectName = await GetCurrentProject();

                if (projectName != null)
                {
                    await memberProjectService.AddNewParticipantChosenMember(chosenUserId, username, projectName.Title);
                }

                SetResponse response = await databaseUtil.CreateConnection().SetAsync($"Role/{nextId}", role);
            }
        }

        public async Task<ProjectModel> GetCurrentProject()
        {
            int? projectId = SessionService.Instance.ProjectId;

            FirebaseResponse response = await databaseUtil.CreateConnection().GetAsync($"Project/{projectId}");
            return JsonConvert.DeserializeObject<ProjectModel>(response.Body);
        }

        private async Task<int> getUserIdChosenForRole(string chosenUser)
        {
            FirebaseResponse response = await databaseUtil.CreateConnection().GetAsync("Member");
            string jsonResponse = response.Body;
            List<Member> members = JsonConvert.DeserializeObject<List<Member>>(jsonResponse);

            if (members != null)
            {
                foreach (var member in members)
                {
                    if (member != null && member.username.Equals(chosenUser, StringComparison.OrdinalIgnoreCase))
                    {
                        return member.Id;
                    }
                }
            }

            return 0;
        }

        private async Task<bool> hasRoleAlready(int userId)
        {

            FirebaseResponse response = await databaseUtil.CreateConnection().GetAsync("Role");
            string jsonResponse = response.Body;
            List<Role> roles = JsonConvert.DeserializeObject<List<Role>>(jsonResponse);

            if (roles != null)
            {
                foreach (var role in roles)
                {
                    if (role != null && role.UserId.Equals(userId) && role.ProjectId.Equals(SessionService.Instance.ProjectId))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public async Task<List<Role>> GetAllMembersInProject()
        {
            FirebaseResponse response = await databaseUtil.CreateConnection().GetAsync("Role/");
            string jsonResponse = response.Body;
            List<Role> roles = JsonConvert.DeserializeObject<List<Role>>(jsonResponse);

            List<Role> roleList = new List<Role>();

            if (roles != null)
            {
                foreach (Role role in roles)
                {
                    if (role != null && role.ProjectId.Equals(SessionService.Instance.ProjectId))
                    {
                        Role currentRole = new Role(role.RoleName)
                        {
                            Id = role.Id,
                            ProjectId = role.ProjectId,
                            Username = role.Username,
                            UserId = role.UserId
                        };

                        roleList.Add(currentRole);
                    }
                }
            }

            return roleList;
        }

        public async Task<Role?> GetRoleById(int id)
        {
            FirebaseResponse response = await databaseUtil.CreateConnection().GetAsync("Role/");
            string jsonResponse = response.Body;
            List<Role> roles = JsonConvert.DeserializeObject<List<Role>>(jsonResponse);

            if (roles != null) 
            {
                foreach (Role role in roles)
                {
                    if (role != null && role.Id.Equals(id))
                    {
                        return role;
                    }
                }
            }

            return null;
        }

        public async Task<string?> GetRoleMemberByUserId(int userId)
        {
            FirebaseResponse response = await databaseUtil.CreateConnection().GetAsync($"Member/");
            string jsonResponse = response.Body;
            List<Member> members = JsonConvert.DeserializeObject<List<Member>>(jsonResponse);

            string? userName = null;

            if (members != null)
            {
                foreach (Member member in members)
                {
                    if (member != null && member.Id.Equals(userId))
                    {
                        userName = member.username;
                    }
                }
            }

            return userName;
        }

        public async Task UpdateRoleOfUser(int roleId, string newRoleName)
        {
            await databaseUtil.CreateConnection().SetAsync($"Role/{roleId}/RoleName", newRoleName);
        }
    }
}
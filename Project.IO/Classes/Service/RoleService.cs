using Project.IO.Utilities;
using Project.IO.Classes.Model;
using FireSharp.Response;
using Newtonsoft.Json;

namespace Project.IO.Classes.Service
{
    public class RoleService
    {

        private DatabaseUtil databaseUtil = new DatabaseUtil();

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

            FirebaseResponse response = await databaseUtil.CreateConnection().GetAsync($"ProjectAssignment");
            string jsonResponse = response.Body;
            List<ProjectAssignment> roles = JsonConvert.DeserializeObject<List<ProjectAssignment>>(jsonResponse);

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

        public async Task<Role?> GetRoleById(int id)
        {
            FirebaseResponse response = await databaseUtil.CreateConnection().GetAsync($"Role/{id}");
            string jsonResponse = response.Body;
            return JsonConvert.DeserializeObject<Role>(jsonResponse);
        }
        public async Task<Role?> GetRoleByName(string name)
        {
            int ProjectId = GetCurrentProject().Id;
            FirebaseResponse response = await databaseUtil.CreateConnection().GetAsync($"Role");
            string jsonResponse = response.Body;
            List<Role> roles = JsonConvert.DeserializeObject<List<Role>>(jsonResponse);
            if(roles!=null)
            {
                foreach (Role role in roles)
                {
                    if(role.RoleName == name && role.ProjectId == ProjectId)
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

        public async Task UpdateRoleOfUser(int roleId, int newRoleName)
        {
            await databaseUtil.CreateConnection().SetAsync($"ProjectAssignment/{roleId}/RoleId", newRoleName);
        }
        public async Task AddRoleToProject(string roleName)
        {
            int nextId = await AutoIncrementRole();
            int ProjectId = GetCurrentProject().Id;
            Role roleToAdd = new Role(roleName,ProjectId)
            {
                Id = nextId
            };
            FirebaseResponse response = await databaseUtil.CreateConnection().GetAsync($"Role");
            string jsonResponse = response.Body;
            List<Role> projectRoles = JsonConvert.DeserializeObject<List<Role>>(jsonResponse);
            if(projectRoles!=null)
            {
                bool roleExists = false;
                foreach (Role role in projectRoles)
                {
                    if (role.RoleName == roleName && role.ProjectId == ProjectId)
                    {
                        roleExists = true;
                        break;
                    }
                }
                if(!roleExists)
                {
                    await databaseUtil.CreateConnection().SetAsync($"Role/{nextId}",roleToAdd);
                }
            }
            else
            {
                await databaseUtil.CreateConnection().SetAsync($"Role/{nextId}", roleToAdd);
            }
        }
        public async Task<List<Role>> getRolesForProject()
        {
            int ProjectId = GetCurrentProject().Id;
            FirebaseResponse response = await databaseUtil.CreateConnection().GetAsync($"Role");
            List<Role> roles = JsonConvert.DeserializeObject<List<Role>>(response.Body);
            List<Role> projectRoles =[];
            foreach (Role role in roles)
            {
                if ( role.ProjectId == ProjectId)
                {
                    projectRoles.Add(role);
                }
            }
            return projectRoles;
        }
        public async Task initializeDefaultRoles(int ProjectId)
        {
            List<string> defaultRoles =
            [
                "voorzitter",
                "vice-voorzitter",
                "notulist",
                "code controleur",
                "planner",
            ];
            foreach(string roleName in defaultRoles)
            {
                int nextId = await AutoIncrementRole();
                Role role = new Role(roleName, ProjectId)
                {
                    Id = nextId
                };
                await databaseUtil.CreateConnection().SetAsync($"Role/{nextId}", role);
            }
        }
    }
}
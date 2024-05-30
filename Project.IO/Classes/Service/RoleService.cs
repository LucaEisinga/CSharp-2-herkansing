using Project.IO.Utilities;
using Project.IO.Classes.Model;
using FireSharp.Response;
using Newtonsoft.Json;

namespace Project.IO.Classes.Service
{
    internal class RoleService
    {

        private DatabaseUtil databaseUtil;

        public async Task<int> AutoIncrementRole()
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

        public async void AddRoleToMember(string username, string roleName)
        {

            if (await hasRoleAlready(await getUserIdChosenForRole(username)))
            {
                int nextId = await AutoIncrementRole();
                int chosenUserId = await getUserIdChosenForRole(username);

                Role role = new Role(roleName);
                role.Id = nextId;
                role.UserId = chosenUserId;

                SetResponse response = await databaseUtil.CreateConnection().SetAsync($"Role/{nextId}", role);
            }
        }

        public async Task<int> getUserIdChosenForRole(string chosenUser)
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

        public async Task<bool> hasRoleAlready(int userId)
        {

            FirebaseResponse response = await databaseUtil.CreateConnection().GetAsync("Role");
            string jsonResponse = response.Body;
            List<Role> roles = JsonConvert.DeserializeObject<List<Role>>(jsonResponse);

            if (roles != null)
            {
                foreach (var role in roles)
                {
                    if (role != null && role.UserId.Equals(userId) && role.ProjectId.Equals(1))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

    }
}

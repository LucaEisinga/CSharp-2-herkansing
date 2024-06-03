using FireSharp.Response;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using Project.IO.Classes.Model;
using Project.IO.Classes.Service;

namespace Project.IO.Utilities
{
    public class AccountUtil
    {
        private DatabaseUtil databaseUtil = new DatabaseUtil();

        public async Task<int> AutoIncrementMember()
        {
            FirebaseResponse response = await databaseUtil.CreateConnection().GetAsync("Member");
            string jsonResponse = response.Body;
            List<Member> members = JsonConvert.DeserializeObject<List<Member>>(jsonResponse);

            int maxId = 0;

            if (members != null)
            {
                foreach (var member in members)
                {
                    if (member != null && member.Id > maxId)
                    {
                        maxId = member.Id;
                    }
                }
            }

            return maxId + 1;
        }

        public async void RegisterNewUser(string userName, string email, string password, string repeatedPassword)
        {

            if (await RegisterUsernameCheck(userName, email))
            {
                int nextId = await AutoIncrementMember();
                Member member = new Member(userName, email, password, repeatedPassword);
                member.Id = nextId;

                SetResponse response = await databaseUtil.CreateConnection().SetAsync($"Member/{nextId}", member);
            }
        }

        public async Task<bool> canLogin(string userName, string password)
        {

            FirebaseResponse response = await databaseUtil.CreateConnection().GetAsync("Member");
            string jsonResponse = response.Body;
            List<Member> members = JsonConvert.DeserializeObject<List<Member>>(jsonResponse);

            if (members != null)
            {
                foreach (var member in members)
                {
                    if (member != null)
                    {
                        if (member.username.Equals(userName) && member.password.Equals(password))
                        {
                            SessionService.Instance.UserId = $"{member.Id}";
                            SessionService.Instance.IsLoggedIn = true ;
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public async Task<bool> RegisterUsernameCheck(string newUsername, string newUserEmail)
        {
            FirebaseResponse response = await databaseUtil.CreateConnection().GetAsync("Member");
            string jsonResponse = response.Body;
            List<Member> members = JsonConvert.DeserializeObject<List<Member>>(jsonResponse);

            if (members != null)
            {
                foreach (var member in members)
                {
                    if (member != null && member.username.Equals(newUsername, StringComparison.OrdinalIgnoreCase) ||
                        member.email.Equals(newUsername, StringComparison.OrdinalIgnoreCase))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}

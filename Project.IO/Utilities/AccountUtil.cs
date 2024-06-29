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

        public async void RegisterNewUser(string userName, string email, string password)
        {

            if (await RegisterUsernameCheck(userName, email))
            {
                int nextId = await AutoIncrementMember();
                Member member = new Member(userName, email, password);
                member.Id = nextId;

                SetResponse response = await databaseUtil.CreateConnection().SetAsync($"Member/{nextId}", member);
            }
        }

        public async Task<Member> GetCurrentLoggedInUserName()
        {
            FirebaseResponse response = await databaseUtil.CreateConnection().GetAsync($"Member/{SessionService.Instance.UserId}");
            string jsonResponse = response.Body;
            Member loggedInMember = JsonConvert.DeserializeObject<Member>(jsonResponse);

            return loggedInMember;
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
                            SessionService.Instance.UserId = member.Id;
                            SessionService.Instance.IsLoggedIn = true;
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

                    if (member != null)
                    {
                        if (member.username.Equals(newUsername, StringComparison.OrdinalIgnoreCase) ||
                                                member.email.Equals(newUsername, StringComparison.OrdinalIgnoreCase))
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        public async Task<Member> GetMemberById(int id)
        {
            FirebaseResponse response = await databaseUtil.CreateConnection().GetAsync($"Member/{id}");
            string jsonResponse = response.Body;
            return JsonConvert.DeserializeObject<Member>(jsonResponse);
        }
        public async Task<Member> getMemberByEmail(string email)
        {
            FirebaseResponse response = await databaseUtil.CreateConnection().GetAsync($"Member");
            string jsonResponse = response.Body;
            List<Member> members = JsonConvert.DeserializeObject<List<Member>>(jsonResponse);
            foreach(Member member in members)
            {
                if(member != null && member.email == email)
                {
                    return member;
                }
            }
            return null;
        }
    }
}

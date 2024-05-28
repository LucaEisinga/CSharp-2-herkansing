using FireSharp.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.IO.Classes;
using FireSharp;
using FireSharp.Interfaces;
using System.Runtime.CompilerServices;
using System.Collections;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Diagnostics;

namespace Project.IO.Utilities
{
    internal class AccountUtil
    {

        private DatabaseUtil databaseUtil = new DatabaseUtil();

        public async Task<int> AutoIncrement()
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

            if (await RegisterUsernameCheck(userName))
            {
                int nextId = await AutoIncrement();
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

            bool loggedIn = false;

            if (members != null)
            {
                foreach (var member in members)
                {
                    if (member != null)
                    {
                        if (member.username.Equals(userName) && member.password.Equals(password))
                        {
                            loggedIn = true;
                        }
                    }
                }
            }

            return loggedIn;
        }

        public async Task<bool> RegisterUsernameCheck(string newUser)
        {
            FirebaseResponse response = await databaseUtil.CreateConnection().GetAsync("Member");
            string jsonResponse = response.Body;
            List<Member> members = JsonConvert.DeserializeObject<List<Member>>(jsonResponse);
            bool isSameUser = false;

            if (members != null)
            {
                foreach (var member in members)
                {
                    if (member != null)
                    {
                        if (!(member.username.Equals(newUser)))
                        {
                            isSameUser = true;
                        }
                        else
                        {
                            isSameUser = false;
                        }
                    }
                }
            }
            else
            {
                isSameUser = true;
            }


            return isSameUser;
        }

    }
}

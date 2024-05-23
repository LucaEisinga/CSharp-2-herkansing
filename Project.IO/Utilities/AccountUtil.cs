using FireSharp.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.IO.Classes;

namespace Project.IO.Utilities
{
    internal class AccountUtil
    {

        private DatabaseUtil databaseUtil = new DatabaseUtil();
        private Member? member;

        public async void RegisterNewUser(string userName, string email, string password, string repeatedPassword)
        {
            this.member = new Member(userName, email, password, repeatedPassword);

            System.Diagnostics.Debug.WriteLine(this.member);

            //moet een manier vinden om een nieuwe id mee te geven, heeft geen automatische increment dus moet zelf een functie maken of zo.

            SetResponse response = await databaseUtil.CreateConnection().SetTaskAsync("User/", this.member);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.IO.Classes.Model
{
    public class Member
    {
        public int Id { get; set; }
        public string username;
        public string email;
        public string password;

        public Member(string username, string email, string password)
        {
            setUsername(username);
            setEmail(email);
            setPassword(password);
        }

        private string getUsername()
        {
            return username;
        }

        private void setUsername(string username)
        {
            if (username != null)
            {
                this.username = username;
            }
        }

        private string getEmail()
        {
            return email;
        }

        private void setEmail(string email)
        {
            if (email != null)
            {
                this.email = email;
            }
        }

        private string getPassword()
        {
            return password;
        }

        private void setPassword(string password)
        {
            if (password != null)
            {
                this.password = password;
            }
        }
    }
}

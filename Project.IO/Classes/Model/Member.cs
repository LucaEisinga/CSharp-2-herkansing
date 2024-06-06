using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.IO.Classes.Model
{
    internal class Member
    {
        public int Id { get; set; }
        public string username;
        public string email;
        public string password;
        public string repeatPassword;

        public Member(string username, string email, string password, string repeatPassword)
        {
            setUsername(username);
            setEmail(email);
            setPassword(password);
            setRepeatPassword(repeatPassword);
        }

        /*private int getId()
        {
            return this.id;
        }

        private void setId(int id)
        {
            if (id != 0)
            {
                this.id = id;
            }
        }*/

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

        private string getRepeatPassword()
        {
            return repeatPassword;
        }

        private void setRepeatPassword(string repeatPassword)
        {
            if (repeatPassword != null)
            {
                this.repeatPassword = repeatPassword;
            }
        }


    }
}

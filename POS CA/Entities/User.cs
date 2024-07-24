using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_CA.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserRole UserRole { get; set; }
        public User(string name, string email, string password, UserRole userRole)
        {
            this.Name = name;
            this.Email = email;
            this.Password = password;
            this.UserRole = userRole;
        }
    }
}

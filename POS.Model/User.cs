using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using POS.Model.Enum;

namespace POS.Model
{
    public class User
    {
        
        [Key]
        [JsonProperty("id")]
        public string id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name can't be longer than 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        [StringLength(100, ErrorMessage = "Email can't be longer than 100 characters.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "User role is required.")]
        public UserRole UserRole { get; set; }

        public User(string name, string email, string password, UserRole userRole)
        {
            Name = name;
            Email = email;
            Password = password;
            UserRole = userRole;
        }

        // Parameterless constructor for EF Core
        public User() { }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS.Model.Enum;

namespace POS.DTO
{
    public class UserDTO
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name can't be longer than 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        [StringLength(100, ErrorMessage = "Email can't be longer than 100 characters.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "User role is required.")]
        public UserRole UserRole { get; set; }

        // Optional constructor for convenience
        public UserDTO() { }

        public UserDTO(string name, string email, string password, UserRole userRole)
        {
            Name = name;
            Email = email;
            UserRole = userRole;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS.Model;
using POS.Model.Enum;

namespace POS.Services.UserServices
{
    public interface IUserService
    {
        Task RegisterUser(User user);
        User AuthenticateUser(string email, string password);
        Task SetUserRole(User user, UserRole role);
        IEnumerable<User> GetAllUsers();
    }
}

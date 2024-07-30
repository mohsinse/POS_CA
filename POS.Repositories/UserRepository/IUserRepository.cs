using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS.Model;
using POS.Model.Enum;

namespace POS.Repositories.UserRepository
{
    public interface IUserRepository
    {
        Task RegisterUser(User user);
        User AuthenticateUser(string email, string password);
        Task SetUserRole(User user, UserRole role);
    }
}

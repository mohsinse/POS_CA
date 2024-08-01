using POS.Model;
using POS.Model.Enum;
using POS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS.Repositories.UserRepository;

namespace POS.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task RegisterUser(User user)
        {
            await _userRepository.RegisterUser(user);
        }

        public User AuthenticateUser(string email, string password)
        {
            return _userRepository.AuthenticateUser(email, password);
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _userRepository.GetAllUsers();
        }

        public async Task SetUserRole(User user, UserRole role)
        {
            await _userRepository.SetUserRole(user, role);
        }
    }
}

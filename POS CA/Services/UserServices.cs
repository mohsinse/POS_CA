using POS_CA.Data;
using POS_CA.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_CA.Services
{
    public class UserServices
    {
        private readonly DataContext _context;

        public UserServices (DataContext context)
        {
            _context = context;
        }

        public async Task RegisterUser(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public User AuthenticateUser(string email, string password)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
        }

        public async Task SetUserRole(User user, UserRole role)
        {
            user.UserRole = role;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}



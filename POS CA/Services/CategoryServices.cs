using Microsoft.EntityFrameworkCore;
using POS_CA.Data;
using POS_CA.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_CA.Services
{
    public class CategoryServices
    {
        private readonly DataContext? _dataContext;
        public CategoryServices(DataContext dataContext) 
        {
            _dataContext = dataContext;
        }
        public async Task<bool> AddCategory(Category category, User user)
        {
            if (user.UserRole == UserRole.Admin && _dataContext != null)
            {
                await _dataContext.Categories.AddAsync(category);
                await _dataContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}

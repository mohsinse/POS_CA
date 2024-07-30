using POS.Model.Enum;
using POS.Model;
using POS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Repositories.CategoryRepository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DBContext? _dataContext;
        public CategoryRepository(DBContext dataContext)
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

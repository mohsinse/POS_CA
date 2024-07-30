using POS.Model.Enum;
using POS.Model;
using POS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS.Repositories.CategoryRepository;

namespace POS.Services.CategoryServices
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public async Task<bool> AddCategory(Category category, User user)
        {
            return await _categoryRepository.AddCategory(category, user);
        }
    }
}

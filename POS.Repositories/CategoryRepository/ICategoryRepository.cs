using POS.Model.Enum;
using POS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Repositories.CategoryRepository
{
    public interface ICategoryRepository
    {
        Task<bool> AddCategory(Category category, User user);
    }
}

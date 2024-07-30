using POS.Model.Enum;
using POS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Services.CategoryServices
{
    public interface ICategoryService
    {
        Task<bool> AddCategory(Category category, User user);
    }
}

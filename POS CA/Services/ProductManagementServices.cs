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
    public class ProductManagementServices
    {
        private readonly DataContext? _dataContext;

        public ProductManagementServices(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<bool> AddProduct(Product product, User user)
        {
            if (user.UserRole == UserRole.Admin && _dataContext != null)
            {
                await _dataContext.Products.AddAsync(product);
                await _dataContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateProduct(Product product, User user)
        {
            if (user.UserRole == UserRole.Admin && _dataContext != null)
            {
                _dataContext.Products.Update(product);
                await _dataContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task RemoveProduct(string productName, User user)
        {
            if (_dataContext != null && user.UserRole == UserRole.Admin)
            {
                Product product = await _dataContext.Products.FirstOrDefaultAsync(x => x.Name == productName);
                _dataContext.Products.Remove(product);
                await _dataContext.SaveChangesAsync();
            }

        }


    }
}

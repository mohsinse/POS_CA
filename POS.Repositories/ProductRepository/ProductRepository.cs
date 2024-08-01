using Microsoft.AspNetCore.Mvc;
using POS.Data;
using POS.Model;
using POS.Model.Enum;

namespace POS.Repositories.ProductRepository
{
    public class ProductRepository
    {
        private readonly DBContext? _dataContext;

        public ProductRepository(DBContext dataContext)
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
                Product product = _dataContext.Products.FirstOrDefault(x => x.Name == productName);
                _dataContext.Products.Remove(product);
                await _dataContext.SaveChangesAsync();
            }

        }

        public ActionResult<IEnumerable<Product>> GetProducts()
        {
            var products = _dataContext.Products.ToList();
            return products;
        }
    }
}

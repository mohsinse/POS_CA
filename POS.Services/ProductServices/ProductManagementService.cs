using Microsoft.AspNetCore.Mvc;
using POS.Data;
using POS.Model;
using POS.Model.Enum;
using POS.Repositories.ProductRepository;

namespace POS.Services.ProductServices
{
    public class ProductManagementService : IProductManagementService
    {
        private readonly IProductRepository _productRepository;

        public ProductManagementService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task<bool> AddProduct(Product product, User user)
        {
            return await _productRepository.AddProduct(product, user);
        }

        public async Task<bool> UpdateProduct(Product product, User user)
        {
            return await _productRepository.UpdateProduct(product,user);
        }

        public async Task RemoveProduct(string productName, User user)
        {
            await _productRepository.RemoveProduct(productName, user);

        }

        public Task<IEnumerable<Product>> GetProducts()
        {
            var products = _productRepository.GetProducts();
            return products;
        }
    }
}

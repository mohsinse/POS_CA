using POS.Model.Enum;
using POS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace POS.Services.ProductServices
{
    public interface IProductManagementService
    {
        Task<bool> AddProduct(Product product, User user);

        Task<bool> UpdateProduct(Product product, User user);

        Task RemoveProduct(string productName, User user);

        ActionResult<IEnumerable<Product>> GetProducts();
    }
}

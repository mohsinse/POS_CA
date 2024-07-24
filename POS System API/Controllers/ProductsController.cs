using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POS_CA.Data;
using POS_CA.Entities;
using POS_CA.Services;

namespace POS_System_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private ProductManagementServices _productManagementServices;
        private readonly DataContext _dataContext;
        private readonly UserServices _userService;

        public ProductsController(ProductManagementServices productManagementServices, UserServices userService, DataContext context)
        {
            _dataContext = context; 
            _productManagementServices = productManagementServices;
            _userService = userService;


            var admin = new User("Admin", "admin@gmail.com", "admin", UserRole.Admin);
            _userService.RegisterUser(admin);

            var cashier = new User("Cashier", "cashier@gmail.com", "cashier", UserRole.Cashier);
            _userService.RegisterUser(cashier);

            var electronicsCategory = new Category { Name = "Electronics" };
            _dataContext.Categories.Add(electronicsCategory);
            _dataContext.SaveChanges();
        }

        //[HttpPost("seedData")]
        //public IActionResult SeedData()
        //{
        //    var admin = new User("Admin", "admin@gmail.com", "admin", UserRole.Admin);
        //    _userService.RegisterUser(admin);

        //    var cashier = new User("Cashier", "cashier@gmail.com", "cashier", UserRole.Cashier);
        //    _userService.RegisterUser(cashier);

        //    var electronicsCategory = new Category { Name = "Electronics" };
        //    _dataContext.Categories.Add(electronicsCategory);
        //    _dataContext.SaveChanges();

        //    return Ok("Data seeded successfully.");
        //}

        [HttpPost("addProduct")]
        public async Task<IActionResult> AddProduct([FromBody] Product product)
        {
            if (product == null)
            {
                return BadRequest("Empty Product");
            }

            var user = _userService.AuthenticateUser("admin@gmail.com", "admin");

            bool result = await _productManagementServices.AddProduct(product, user);
            if (result)
            {
                return Ok("Product added successfully.");
            }
            else
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

        [HttpGet("getProducts")]
        public ActionResult<IEnumerable<Product>> GetProducts()
        {
            var products = _dataContext.Products.ToList();
            return Ok(products);
        }


        [HttpDelete("deleteProduct/{productName}")]
        public async Task<IActionResult> DeleteProduct(string productName)
        {
            var user = _userService.AuthenticateUser("admin@gmail.com", "admin");
            await _productManagementServices.RemoveProduct(productName, user);
            return Ok("Product deleted successfully.");
        }

        [HttpPut("editProduct/{updatedProduct}")]
        public async Task<IActionResult> EditProduct([FromBody] Product updatedProduct)
        {
            if (updatedProduct == null)
            {
                return BadRequest("Empty Product");
            }

            var user = _userService.AuthenticateUser("admin@gmail.com", "admin");

            bool result = await _productManagementServices.UpdateProduct(updatedProduct, user);
            if (result)
            {
                return Ok("Product updated successfully.");
            }
            else
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

    }
}

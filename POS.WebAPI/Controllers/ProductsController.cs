﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POS.Services.ProductServices;
using POS.Services.UserServices;
using POS.Data;
using POS.Model;
using POS.Model.Enum;
using Microsoft.Identity.Web.Resource;

namespace POS_System_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductManagementService _productManagementServices;
        private readonly IUserService _userService;


        public ProductsController(IProductManagementService productManagementServices, IUserService userService)
        {
            _productManagementServices = productManagementServices;
            _userService = userService;


            //var admin = new User("Admin", "admin@gmail.com", "admin", UserRole.Admin);
            //_userService.RegisterUser(admin);

            //var cashier = new User("Cashier", "cashier@gmail.com", "cashier", UserRole.Cashier);
            //_userService.RegisterUser(cashier);

            //var electronicsCategory = new Category { Name = "Electronics" };
            //_dataContext.Categories.Add(electronicsCategory);
            //_dataContext.SaveChanges();
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

            var user = _userService.AuthenticateUser("user@example.com", "string");

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

        [RequiredScope("Products.Read")]
        [HttpGet("getProducts")]
        public Task<IEnumerable<Product>> GetProducts()
        {
            var products = _productManagementServices.GetProducts();
            return products;
        }


        [HttpDelete("deleteProduct/{productName}")]
        public async Task<IActionResult> DeleteProduct(string productName)
        {
            var user = _userService.AuthenticateUser("user@example.com", "string");
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

            var user = _userService.AuthenticateUser("user@example.com", "string");

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

using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using POS_CA.Data;
using POS_CA.Entities;
using POS_CA.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace POS_System_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {

        private ProductManagementServices _productManagementServices;
        private readonly DataContext _dataContext;
        private readonly UserServices _userService;

        public AuthenticationController(ProductManagementServices productManagementServices, UserServices userService, DataContext context)
        {
            _dataContext = context;
            _productManagementServices = productManagementServices;
            _userService = userService;

            var admin = new User("Admin", "admin@gmail.com", "admin", UserRole.Admin);
            _userService.RegisterUser(admin);

            var cashier = new User("Cashier", "cashier@gmail.com", "cashier", UserRole.Cashier);
            _userService.RegisterUser(cashier);

            var fashionCategory = new Category { Name = "Fashion" };
            _dataContext.Categories.Add(fashionCategory);
            _dataContext.SaveChanges();
        }



        [HttpGet("{email}/{password}")]
        public async Task<ActionResult<string>> Login(string email, string password)
        {
            // Check the login credentials from the users table
            var user = _userService.AuthenticateUser(email, password);
            if (user == null)
            {
                return Unauthorized("Invalid email or password.");
            }
            var token = GenerateJwtToken(user);
            // Return the user's role
            return Ok($"Sucessfully Logged in : {token}");
        }


        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("eyJhbGciOiJIUzI1NiJ9.eyJSb2xlIjoiQWRtaW4iLCJJc3N1ZXIiOiJJc3N1ZXIiLCJVc2VybmFtZSI6IkphdmFJblVzZSIsImV4cCI6MTcyMTc0NjE1NiwiaWF0IjoxNzIxNzQ2MTU2fQ.nKTyShgurV7Jy3yY_awDf-khRZDxMq8JpuTN0b7nGFE"));
            var clims = new[]
       {
            new Claim(ClaimTypes.Name, user.Name),
        };

            //var tokenDescriptor = new SecurityTokenDescriptor
            //{
            //    Subject = new ClaimsIdentity(new[]
            //    {
            //    new Claim(ClaimTypes.Name, user.Name),
                
            //    // Add other claims as needed
            //}),
               
            //    Expires = DateTime.UtcNow.AddHours(1),
            //    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            //    Issuer="",
            //    Audience=""
            //};

            var token = new JwtSecurityToken(
           issuer: "http://localhost:5082",
           audience: "http://localhost:5082",
           claims: clims,
           expires: DateTime.Now.AddDays(30),
           signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
           );
            string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);

            // var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenAsString; //tokenHandler.WriteToken(token);
        }
    }
}

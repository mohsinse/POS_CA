using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using POS.Services;
using POS.Services.ProductServices;
using POS.Services.UserServices;
using POS.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace POS_System_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService _userService;
        public AuthenticationController(IUserService userService)
        {
            _userService = userService;
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
            //var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("eyJhbGciOiJIUzI1NiJ9.eyJSb2xlIjoiQWRtaW4iLCJJc3N1ZXIiOiJJc3N1ZXIiLCJVc2VybmFtZSI6IkphdmFJblVzZSIsImV4cCI6MTcyMTc0NjE1NiwiaWF0IjoxNzIxNzQ2MTU2fQ.nKTyShgurV7Jy3yY_awDf-khRZDxMq8JpuTN0b7nGFE"));
            var clims = new[]
            {
                new Claim(ClaimTypes.Name, user.Name),
            };

            var token = new JwtSecurityToken(
                issuer: "http://localhost:5082",
                audience: "http://localhost:5082",
                claims: clims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
           );
            string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenAsString;
        }
    }
}

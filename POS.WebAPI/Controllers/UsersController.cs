using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POS.Data;
using POS.Model;
using POS.Services;
using POS.Services.UserServices;
using POS.Model.Enum;
using AutoMapper;
using POS.DTO;

namespace POS.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DBContext _dataContext;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService, DBContext context, ILogger<UsersController> logger, IMapper mapper)
        {
            _dataContext = context;
            _userService = userService;
            _mapper = mapper;


            var admin = new User("Admin", "admin@gmail.com", "admin", UserRole.Admin);
            _userService.RegisterUser(admin);

            var cashier = new User("Cashier", "cashier@gmail.com", "cashier", UserRole.Cashier);
            _userService.RegisterUser(cashier);

            var fashionCategory = new Category { Name = "Fashion" };
            _dataContext.Categories.Add(fashionCategory);
            _dataContext.SaveChanges();
            _logger = logger;
        }

        [HttpPost("RegisterUser")]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            await _userService.RegisterUser(user);
            return Ok();
        }

        [Authorize]
        [HttpGet("GetAllUsers")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            _logger.LogInformation("----GetAllUsers Started------");
            if (_dataContext.Users == null)
            {
                return NotFound();
            }

            
            var users = await _dataContext.Users.ToListAsync();
            _logger.LogInformation("----GetAllUsers End------");
            return Ok(users.Select(user => _mapper.Map<UserDTO>(user)));
            //return users;
        }


        [HttpPut("SetUserRole")]
        public async Task<IActionResult> UpdateUserRole(string userEmail, string role)
        {
            var user = _dataContext.Users.FirstOrDefault(u => u.Email == userEmail);
            if (user == null)
            {
                return NotFound();
            }
            if (role != null)
            {
                if (role == "Admin" ||  role == "admin")
                {
                    user.UserRole = UserRole.Admin;
                }
                else if (role == "Cashier" || role == "cashier")
                {
                    user.UserRole = UserRole.Cashier;
                }
            }

            
            await _dataContext.SaveChangesAsync();

            return Ok(new
            {
                user.Id,
                user.Name,
                user.Email,
                user.UserRole
            });
        }



        //[HttpPost("RegisterUser")]
        //public async Task<IActionResult> RegisterUser([FromBody] User user)
        //{
        //    if (product == null)
        //    {
        //        return BadRequest("Empty Product");
        //    }

        //    var user = _userService.AuthenticateUser("admin@gmail.com", "admin");

        //    bool result = await _productManagementServices.AddProduct(product, user);
        //    if (result)
        //    {
        //        return Ok("Product added successfully.");
        //    }
        //    else
        //    {
        //        return StatusCode(500, "A problem happened while handling your request.");
        //    }
        //}
    }
}

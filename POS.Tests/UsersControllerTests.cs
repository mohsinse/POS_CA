//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//using Moq;
//using FluentAssertions;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using AutoMapper;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using NUnit.Framework;
//using POS.Data;
//using POS.Services.UserServices;
//using POS.WebAPI.Controllers;
//using POS.Model;
//using POS.DTO;
//using POS.Model.Enum;
//using Microsoft.EntityFrameworkCore;

//namespace POS.Tests
//{
//    [TestFixture]
//    public class UsersControllerTests
//    {
//        private Mock<IUserService> _userServiceMock;
//        private Mock<DBContext> _dataContextMock;
//        private Mock<ILogger<UsersController>> _loggerMock;
//        private IMapper _mapper;
//        private UsersController _controller;

//        [SetUp]
//        public void Setup()
//        {
//            _userServiceMock = new Mock<IUserService>();
//            _dataContextMock = new Mock<DBContext>();
//            _loggerMock = new Mock<ILogger<UsersController>>();

//            var mapperConfig = new MapperConfiguration(cfg =>
//            {
//                cfg.CreateMap<User, UserDTO>();
//                cfg.CreateMap<UserDTO, User>();
//            });
//            _mapper = mapperConfig.CreateMapper();

//            _controller = new UsersController(_userServiceMock.Object, _dataContextMock.Object, _loggerMock.Object, _mapper);
//        }

//        [Test]
//        public async Task PostUser_ShouldReturnOk()
//        {
//            // Arrange
//            var user = new User("John Doe", "john.doe@example.com", "password", UserRole.Cashier);

//            // Act
//            var result = await _controller.PostUser(user);

//            // Assert
//            result.Should().BeOfType<OkResult>();
//        }

//        //[Test]
//        //public async Task GetUsers_ShouldReturnOk_WithUserList()
//        //{
//        //    // Arrange
//        //    var users = new List<User>
//        //{
//        //    new User("John Doe", "john.doe@example.com", "password", UserRole.Cashier),
//        //    new User("Jane Smith", "jane.smith@example.com", "password", UserRole.Admin)
//        //};
//        //    _dataContextMock.Setup(db => db.Users.ToListAsync())
//        //        .ReturnsAsync(users);

//        //    // Act
//        //    var result = await _controller.GetUsers() as OkObjectResult;
//        //    var userDTOs = result.Value as IEnumerable<UserDTO>;

//        //    // Assert
//        //    result.Should().NotBeNull();
//        //    userDTOs.Should().HaveCount(users.Count);
//        //    userDTOs.First().Name.Should().Be(users.First().Name);
//        //}

//        [Test]
//        public async Task UpdateUserRole_ShouldReturnOk_WhenUserExists()
//        {
//            // Arrange
//            var user = new User("John Doe", "john.doe@example.com", "password", UserRole.Cashier);
//            _dataContextMock.Setup(db => db.Users.FirstOrDefault(It.IsAny<System.Linq.Expressions.Expression<System.Func<User, bool>>>())).Returns(user);

//            // Act
//            var result = await _controller.UpdateUserRole("john.doe@example.com", "Admin") as OkObjectResult;
//            var updatedUser = result.Value as dynamic;

//            // Assert
//            result.Should().NotBeNull();
//            updatedUser.UserRole.Should().Be(UserRole.Admin);
//        }

//        [Test]
//        public async Task UpdateUserRole_ShouldReturnNotFound_WhenUserDoesNotExist()
//        {
//            // Arrange
//            _dataContextMock.Setup(db => db.Users.FirstOrDefault(It.IsAny<System.Linq.Expressions.Expression<System.Func<User, bool>>>())).Returns((User)null);

//            // Act
//            var result = await _controller.UpdateUserRole("nonexistent@example.com", "Admin");

//            // Assert
//            result.Should().BeOfType<NotFoundResult>();
//        }
//    }
//}

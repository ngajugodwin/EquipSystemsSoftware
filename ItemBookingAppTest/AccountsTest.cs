using AutoMapper;
using CloudinaryDotNet.Actions;
using ItemBookingApp_API.Areas.Resources.AppUser;
using ItemBookingApp_API.Areas.SuperAdmin.Controllers;
using ItemBookingApp_API.Controllers;
using ItemBookingApp_API.Domain.Models.Identity;
using ItemBookingApp_API.Domain.Models.OrderAggregate;
using ItemBookingApp_API.Domain.Services;
using ItemBookingApp_API.Domain.Services.Communication;
using ItemBookingApp_API.Resources.Auth;
using Microsoft.AspNetCore.Http;
using Moq;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace ItemBookingAppTest
{
    public class AccountsTest
    {
        public Mock<IUserService> mock = new Mock<IUserService>();
        public Mock<IAuthService> mockAuthService = new Mock<IAuthService>();
        public Mock<IMapper> mapper = new Mock<IMapper>();

        [Fact]
        public async void Login_Test()
        {
            // preparet test user data
            var user = new UserResource { Id = 1, Email = "test@example.com" };

            //prepare token response with test data
            var token = new TokenResource 
            { 
                RefreshToken = "teteiw7wefwefwefhyw78ie" , 
                User = user, 
                Token = "ee78wefwefweetee8eje", 
                Expiration = DateTime.Now 
            };

            //mockup authentication service to call login function
            mockAuthService.Setup(p => p.LoginAsync(user.Email, "password")).ReturnsAsync(new AuthResponse(token));

            AuthController authController = new AuthController(mockAuthService.Object);

            // call controller to test the login function with test data
            var result = await authController.LoginAsync(
                new LoginResource 
                { 
                    Email = "test@example.com", 
                    Password = "password"
                });

            Assert.NotNull(result); // Assert and confirm the user object was found and not null
        }

        //[Fact]
        //public async void RegisterNewIndividualAccount_Test()
        //{

        //    var roles = new List<string>();            
        //    roles.Add("Owner");

        //    var content = "Hello World from a Fake File";
        //    var fileName = "meansOfIdentification.jpeg";
        //    var stream = new MemoryStream();
        //    var writer = new StreamWriter(stream);
        //    writer.Write(content);
        //    writer.Flush();
        //    stream.Position = 0;

        //    IFormFile file = new FormFile(stream, 0, stream.Length, "id_from_form", fileName);

        //    var newUser = new AppUser
        //    {
        //        Id = 1,
        //        FirstName = "Cyndy",
        //        LastName = "Test",
        //        Email = "cyndy@example.com",
        //        City = "Cov",
        //        State = "Coventry",
        //        CreatedAt = DateTime.Now,               
        //    };

        //    var cyndy = new SaveUserResource
        //    {
        //        FirstName = "Cyndy",
        //        LastName = "Test",
        //        Email = "cyndy@example.com",
        //        City = "Cov",
        //        State = "Coventry",
        //        AccountType = 1,
        //        IsExternalReg = true,
        //        File = file,
        //        Password = "Password"
        //    };


        //    mock.Setup(p => p.SaveAsync(newUser, true, roles, "Password123", file)).ReturnsAsync(new UserResponse(newUser));

        //    AccountsController accountsController = new AccountsController(mapper.Object, mock.Object);

        //    var result = await accountsController.CreateNewAccountAsync(cyndy);

        //    Assert.NotNull(result);
        //}
    }
}

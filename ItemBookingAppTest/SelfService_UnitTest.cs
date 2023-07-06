using AutoMapper;
using ItemBookingApp_API.Controllers;
using ItemBookingApp_API.Domain.Models;
using ItemBookingApp_API.Domain.Models.Identity;
using ItemBookingApp_API.Domain.Services;
using ItemBookingApp_API.Domain.Services.Communication;
using Microsoft.Extensions.Logging;
using ItemBookingApp_API.Resources.SelfService;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemBookingAppTest
{   
    public class SelfService_UnitTest
    {
        public Mock<IUserService> mock = new Mock<IUserService>();
        public Mock<IMapper> mapper = new Mock<IMapper>();

        [Fact]
        public async void ChangeUserPassword_Test()
        {
            // prepare test data
            var user = new AppUser
            {
               Id = 1,
               Email = "test@example.com",
               Status = EntityStatus.Active
            };

            // 
            mock.Setup(p => p.GetUserByIdAsync(1)).ReturnsAsync(new UserResponse(user));

            // test the change password function with test data
            mock.Setup(p => p.ChangePassword(1, "password", "newPassword", false))
                .ReturnsAsync(new UserResponse(user));

            SelfServicesController selfServiceController = new SelfServicesController(mapper.Object, mock.Object);

            // test fake data using from the self service controller
            var result = await selfServiceController
                .ChangePasswordAsync(1, new PasswordUpdateResource
                {
                    OldPassword = "password",
                    NewPassword = "newPassword"
                });

            Assert.NotNull(result); // Assert that a user object was returned once password is changed successfully
        }

    }
}

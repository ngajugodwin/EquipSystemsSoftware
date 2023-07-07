using AutoMapper;
using ItemBookingApp_API.Areas.SuperAdmin.Controllers;
using ItemBookingApp_API.Domain.Models;
using ItemBookingApp_API.Domain.Models.OrderAggregate;
using ItemBookingApp_API.Domain.Services;
using ItemBookingApp_API.Domain.Services.Communication;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemBookingAppTest
{
    public class ManageCategory_UnitTest
    {
        public Mock<ICategoryService> mock = new Mock<ICategoryService>();

        public Mock<IMapper> mapper = new Mock<IMapper>();

        [Fact]
        public async void GetOneCategory_Unit_Test()
        {
            // prepare test data
            var category = new Category 
            { 
                Id = 1, 
                Name = "Adventure", 
                CreatedAt = DateTime.Now 
            };

            // fake and setup category service to call a function
            mock.Setup(p => p.GetCategoryByIdAsync(1)).ReturnsAsync(new CategoryResponse(category));

            ManageCategoriesController categoryController = new ManageCategoriesController(mapper.Object, mock.Object);

            //mock controller and call getCategory function
            var result = await categoryController.GetCategoryAsync(1);

            // Assert that the category got a result and is not null
            Assert.NotNull(result);
        }
    }
}

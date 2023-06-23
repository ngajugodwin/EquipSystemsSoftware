using AutoMapper;
using ItemBookingApp_API.Domain.Models.Identity;
using ItemBookingApp_API.Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace ItemBookingApp_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SelfServicesController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public SelfServicesController(IMapper mapper,
        IUserService userService)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet("getUserAddress/{userId}")]
        public async Task<IActionResult> GetUserAddress(long userId)
        {
            var userAddressToReturn = await _userService.GetUserAddress(userId);

            return Ok(userAddressToReturn);
        }

        [HttpPut("updateUserAddress/{userId}")]
        public async Task<IActionResult> UpdateUserAddress(long userId, UserAddress userAddress)
        {
            var updatedUserAddress = await _userService.UpdateUserAddress(userId, userAddress);

            return Ok(updatedUserAddress);
        }


        [HttpGet]
        public async Task<IActionResult> GetUserProfile(int userId)
        {
            int [] marks = new int[]  { 99,  98, 92, 97, 95, 20};
            
            return Ok(marks.ToList());
        }

    }
}

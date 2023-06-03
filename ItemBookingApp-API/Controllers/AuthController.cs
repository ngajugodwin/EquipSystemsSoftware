using ItemBookingApp_API.Domain.Services;
using ItemBookingApp_API.Extension;
using ItemBookingApp_API.Resources.Auth;
using ItemBookingApp_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ItemBookingApp_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginResource loginResource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var result = await _authService.LoginAsync(loginResource.Email.ToLower(), loginResource.Password);

            if (!result.Success)
                return Unauthorized(result.Message);

            return Ok(result.Resource);
        }

        [HttpPost("refreshToken")]
        public async Task<IActionResult> RefreshTokenAsync([FromBody] TokenRequestResource tokenRequestResource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var result = await _authService.RefreshTokenAsync(tokenRequestResource.RefreshToken);

            if (!result.Success)
                return Unauthorized(result.Message);

            return Ok(result.Resource);
        }
    }
}

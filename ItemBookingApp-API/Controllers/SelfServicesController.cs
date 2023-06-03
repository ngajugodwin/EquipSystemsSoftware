using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace ItemBookingApp_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SelfServicesController : ControllerBase
    {
        public SelfServicesController()
        {

        }

        [HttpGet]
        public async Task<IActionResult> GetUserProfile(int userId)
        {
            int [] marks = new int[]  { 99,  98, 92, 97, 95, 20};
            
            return Ok(marks.ToList());
        }
    }
}

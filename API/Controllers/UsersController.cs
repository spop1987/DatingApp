using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserServices _userServices;

        public UsersController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById(int userId)
        {
            return Ok(await _userServices.GetUserById(userId));
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            return Ok(await _userServices.GetUsers());
        }
    }
}
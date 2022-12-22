using API.Data.Dtos;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IAccountService _accountService;

        public AccountController(ILogger<AccountController> logger, IAccountService accountService)
        {
            _logger = logger;
            _accountService = accountService;
        }

        [HttpPost("register")]
        public async Task<UserDto> Register([FromBody] RegisterDto registerDto)
        {
            return await _accountService.Register(registerDto);
        }

        [HttpPost("login")]
        public async Task<UserDto> Login(LoginDto loginDto)
        {
            return await _accountService.Login(loginDto);
        }
    }
}
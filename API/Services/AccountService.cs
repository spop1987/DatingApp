using System.Net;
using System.Security.Cryptography;
using System.Text;
using API.Data.DataAccess;
using API.Data.Dtos;
using API.Data.Entities;
using API.Data.Translators;
using API.Helpers;

namespace API.Services
{
    public interface IAccountService
    {
        Task<UserDto> Login(LoginDto loginDto);
        Task<UserDto> Register(RegisterDto registerDto);
    }
    public class AccountService : IAccountService
    {
        private readonly ICommands _commands;
        private readonly IQueries _queries;
        private readonly IToDtoTranslator _toDtoTranslator;

        public AccountService(ICommands commands, IQueries queries, IToDtoTranslator toDtoTranslator)
        {
            _commands = commands;
            _queries = queries;
            _toDtoTranslator = toDtoTranslator;
        }

        public async Task<UserDto> Login(LoginDto loginDto)
        {
            var user = await _queries.GetUserByUserNameAsync(loginDto.Username);

            if(user is null) throw new Exception("Unauthorized: Invalid UserName");

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for(int i = 0; i < computedHash.Length; i++)
            {
                if(computedHash[i] != user.PasswordHash[i]) throw new Exception("Unauthorized: Invalid Passwrord");
            }

            return await _toDtoTranslator.ToUserDto(user);
        }

        public async Task<UserDto> Register(RegisterDto registerDto)
        {
            if(string.IsNullOrEmpty(registerDto.Username.Trim()))
                throw new DatingAppInvalidUserException(new Error 
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Username is null or empty"
                });
            
            if(string.IsNullOrEmpty(registerDto.Password))
                throw new DatingAppInvalidUserException(new Error 
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Password is null or empty"
                });

            if(await _queries.UserExistsByUserNameAsync(registerDto.Username)){
                throw new DatingAppInvalidUserException(new Error 
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Username already exists in database"
                });
            }
                
            using var hmac = new HMACSHA512();

            var user = new AppUser
            {
                UserName = registerDto.Username.ToUpper(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };

            await _commands.SaveUserAsync(user);
            return await _toDtoTranslator.ToUserDto(user);
        }
    }
}
using System.Security.Cryptography;
using System.Text;
using API.Data.DataAccess;
using API.Entities;

namespace API.Services
{
    public interface IAccountService
    {
        Task<AppUser> Register(string userName, string password);
    }
    public class AccountService : IAccountService
    {
        private readonly ICommands _commands;

        public AccountService(ICommands commands)
        {
            _commands = commands;
        }

        public async Task<AppUser> Register(string userName, string password)
        {
            using var hmac = new HMACSHA512();

            var user = new AppUser
            {
                UserName = userName,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)),
                PasswordSalt = hmac.Key
            };

            await _commands.SaveUserAsync(user);
            return user;
        }
    }
}
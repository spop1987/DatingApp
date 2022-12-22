using API.Data.Dtos;
using API.Data.Entities;
using API.Interfaces;

namespace API.Data.Translators
{
    public interface IToDtoTranslator
    {
        UserDto ToUserDto(AppUser user);
    }
    public class ToDtoTranslator : IToDtoTranslator
    {
        private readonly ITokenService _tokenService;

        public ToDtoTranslator(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }
        public UserDto ToUserDto(AppUser user)
        {
            return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }
    }
}
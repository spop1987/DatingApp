using System.ComponentModel;
using System.Runtime.CompilerServices;
using API.Data.Dtos;
using API.Data.Entities;
using API.Interfaces;

namespace API.Data.Translators
{
    public interface IToDtoTranslator
    {
        Task<UserDto> ToUserDto(AppUser user);
        Task<MemberDto> ToMemberDto(AppUser user);
        Task<List<MemberDto>> ToMembersDto(IEnumerable<AppUser> users);

    }
    public class ToDtoTranslator : IToDtoTranslator
    {
        private readonly ITokenService _tokenService;

        public ToDtoTranslator(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }
        public async Task<UserDto> ToUserDto(AppUser user)
        {
            return await Task.FromResult(new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            });
        }

        public async Task<MemberDto> ToMemberDto(AppUser user)
        {
            return await Task.FromResult(new MemberDto{
                UserId = user.UserId,
                UserName = user.UserName,
                City = user.City,
                Country = user.Country,
                Created = user.Created,
                DateOfBirth = user.DateOfBirth,
                Gender = user.Gender,
                Interests = user.Interests,
                Introduction = user.Introduction,
                KnownAs = user.KnownAs,
                LastActive = user.LastActive,
                LookingFor = user.LookingFor,
                Photos = ToPhotosDto(user.Photos)
            });
        }

        private List<PhotoDto> ToPhotosDto(IEnumerable<Photo> photos)
        {
            if(photos == null)
                return null;

            var phtosDto = new List<PhotoDto>();

            photos.ToList().ForEach(p => 
            {
                phtosDto.Add(
                    new PhotoDto {
                        Url = p.Url,
                        IsMain = p.IsMain,
                        PhotoId = p.PhotoId
                    }
                );
            });
               
            return phtosDto;
        }

        public async Task<List<MemberDto>> ToMembersDto(IEnumerable<AppUser> users)
        {
            var membersDto = new List<MemberDto>();

            var tasks = new List<Task>();

            users.ToList().ForEach(user => {
                tasks.Add(Task.Run(() => {
                    membersDto.Add(new MemberDto{
                        UserId = user.UserId,
                        UserName = user.UserName,
                        City = user.City,
                        Country = user.Country,
                        Created = user.Created,
                        DateOfBirth = user.DateOfBirth,
                        Gender = user.Gender,
                        Interests = user.Interests,
                        Introduction = user.Introduction,
                        KnownAs = user.KnownAs,
                        LastActive = user.LastActive,
                        LookingFor = user.LookingFor,
                        Photos = ToPhotosDto(user.Photos)   
                    });
                }));
            });
            Task t = Task.WhenAll(tasks);
            await t;

            return membersDto;
        }
    }
}
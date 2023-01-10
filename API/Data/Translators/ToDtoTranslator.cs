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
                PhotoUrl = user.Photos.First(p => p.IsMain is true).Url,
                City = user.City,
                Country = user.Country,
                Created = user.Created,
                Age = user.GetAge(),
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
            if(users == null)
                return null;

            var membersDto = new List<MemberDto>();
            
            var tasks = new List<Task>();
            
            users.ToList().ForEach(user => {
            tasks.Add(Task.Run(async () => {
                membersDto.Add(await ToMemberDto(user));
                }));
            });

            Task t = Task.WhenAll(tasks);
            await t;
          
            return membersDto;
        }
    }
}
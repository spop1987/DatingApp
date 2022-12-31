using API.Data.DataAccess;
using API.Data.Dtos;
using API.Data.Entities;
using API.Data.Translators;

namespace API.Services
{
    public interface IUserServices
    {
        Task<MemberDto> GetUserById(int userId);
        Task<IEnumerable<MemberDto>> GetUsers();
    }

    public class UserServices : IUserServices
    {
        private readonly IQueries _queries;
        private readonly IToDtoTranslator _toDtoTranslator;

        public UserServices(IQueries queries, IToDtoTranslator toDtoTranslator)
        {
            _queries = queries;
            _toDtoTranslator = toDtoTranslator;
        }
        public async Task<IEnumerable<MemberDto>> GetUsers()
        {
            var users = await _queries.GetUsersAsync();
            return await _toDtoTranslator.ToMembersDto(users);
        }

        public async Task<MemberDto> GetUserById(int userId)
        {
            var user = await _queries.GetUserByIdAsync(userId);
            return await _toDtoTranslator.ToMemberDto(user);
        }
    }
}
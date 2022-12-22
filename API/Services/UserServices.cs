using API.Data.DataAccess;
using API.Data.Entities;

namespace API.Services
{
    public interface IUserServices
    {
        Task<AppUser> GetUserById(int userId);
        Task<IEnumerable<AppUser>> GetUsers();
    }

    public class UserServices : IUserServices
    {
        private readonly IQueries _queries;

        public UserServices(IQueries queries)
        {
            _queries = queries;
        }
        public async Task<IEnumerable<AppUser>> GetUsers()
        {
            return await _queries.GetUsersAsync();
        }

        public async Task<AppUser> GetUserById(int userId)
        {
            return await _queries.GetUserByIdAsync(userId);
        }
    }
}
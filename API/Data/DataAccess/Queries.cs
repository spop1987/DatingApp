using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data.DataAccess
{
    public interface IQueries
    {
        Task<IEnumerable<AppUser>> GetUsersAsync();
        Task<AppUser> GetUserByIdAsync(int userId); 
    }

    public class Queries : IQueries
    {
        private readonly DataContext _dataContext;

        public Queries(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<AppUser> GetUserByIdAsync(int userId)
        {
            var user =  await _dataContext.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if(user is null)
                throw new Exception("User not found");
            
            return user;
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            var users = await _dataContext.Users.ToListAsync();

            if(!users.Any())
                throw new Exception("There are not Users in DB");
            
            return users;
        }
    }
}
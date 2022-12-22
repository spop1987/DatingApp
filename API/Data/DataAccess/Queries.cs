using API.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data.DataAccess
{
    public interface IQueries
    {
        Task<IEnumerable<AppUser>> GetUsersAsync();
        Task<AppUser> GetUserByIdAsync(int userId);
        Task<bool> UserExistsByUserNameAsync(string userName);

        Task<AppUser> GetUserByUserNameAsync(string userName);
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

        public async Task<bool> UserExistsByUserNameAsync(string userName)
        {
            return await _dataContext.Users.AnyAsync(u => u.UserName.ToUpper() == userName.ToUpper());
        }

        public async Task<AppUser> GetUserByUserNameAsync(string userName)
        {
            return await _dataContext.Users.SingleOrDefaultAsync(u => u.UserName.ToUpper() == userName.ToUpper());
        }
    }
}
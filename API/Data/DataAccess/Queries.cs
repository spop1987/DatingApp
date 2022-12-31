using API.Data.Entities;
using API.Helpers;
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
            var user =  await UserWithInclude().FirstOrDefaultAsync(u => u.UserId == userId);
            if(user is null)
                throw new DatingAppNotFoundUserException(new Error{
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "User not found"
                });
            
            return user;
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            var users = await UserWithInclude().Include(u => u.Photos).ToListAsync();

            if(!users.Any())
                throw new DatingAppNotFoundUsersException(new Error{
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "There are not Users in DB"
                });
            
            return users;
        }

        public async Task<bool> UserExistsByUserNameAsync(string userName)
        {
            return await UserWithInclude().AnyAsync(u => u.UserName.ToUpper() == userName.ToUpper());
        }

        public async Task<AppUser> GetUserByUserNameAsync(string userName)
        {
            try
            {
                var user = await UserWithInclude()
                .SingleOrDefaultAsync(u => u.UserName.ToUpper() == userName.ToUpper());
            
                return user;
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return (AppUser)null;
            }
        }

        private IQueryable<AppUser> UserWithInclude()
        {
            return _dataContext.Users
                .Include(u => u.Photos).AsQueryable();
        }
    }
}
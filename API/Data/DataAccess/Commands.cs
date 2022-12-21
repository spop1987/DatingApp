using API.Entities;

namespace API.Data.DataAccess
{
    public interface ICommands
    {
        Task SaveUserAsync(AppUser user);
    }

    public class Commands : ICommands
    {
        private readonly DataContext _context;

        public Commands(DataContext context)
        {
            _context = context;
        }
        public async Task SaveUserAsync(AppUser user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
    }
}
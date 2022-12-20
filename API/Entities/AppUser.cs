using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class AppUser
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
    }
}
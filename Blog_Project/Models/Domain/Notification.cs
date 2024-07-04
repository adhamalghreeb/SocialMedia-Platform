using Microsoft.AspNetCore.Identity;

namespace Blog_Project.Models.Domain
{
    public class Notification
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public IdentityUser User { get; set; }
        public string Message { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsRead { get; set; }
    }
}

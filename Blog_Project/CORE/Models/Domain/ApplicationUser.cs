using Microsoft.AspNetCore.Identity;

namespace Blog_Project.CORE.Models.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<follow> Followers { get; set; }
        public ICollection<follow> Following { get; set; }
        public ICollection<Notification> Notifications { get; set; }
    }
}

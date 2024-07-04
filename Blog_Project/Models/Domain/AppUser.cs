using Microsoft.AspNetCore.Identity;

namespace Blog_Project.Models.Domain
{
    public class AppUser : IdentityUser
    {
        public ICollection<UserPermissions> userPermissions { get; set; }
        public ICollection<Comment> comments { get; set; }
        public ICollection<follow> Followers { get; set; }
        public ICollection<follow> Followees { get; set; }

    }
}

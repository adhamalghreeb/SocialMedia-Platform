using Microsoft.AspNetCore.Identity;

namespace Blog_Project.Models.Domain
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
        public Guid BlogPostId { get; set; }
        public BlogPost BlogPost { get; set; }
        public string UserId { get; set; }
        public IdentityUser User { get; set; }
    }
}

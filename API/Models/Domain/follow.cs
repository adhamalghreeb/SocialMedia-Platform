using Microsoft.AspNetCore.Identity;

namespace Blog_Project.Models.Domain
{
    public class follow
    {
        public string FollowerId { get; set; }
        public AppUser Follower { get; set; }

        public string FolloweeId { get; set; }
        public AppUser Followee { get; set; }
    }
}

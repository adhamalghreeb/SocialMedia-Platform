using Microsoft.AspNetCore.Identity;
using System.Net.Sockets;

namespace Blog_Project.Models.Domain
{
    public class UserPermissions
    {
        public string UserId { get; set; }
        public Permission PermissionId { get; set; }
        public AppUser User { get; set; }
    }
}
using Blog_Project.CORE.Models.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Blog_Project.EF.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> dbContextOptions) : base(dbContextOptions) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Create Reader and Writer Role
            var ReaderRoleId = "6b9faf15-a653-486d-8faf-e6ef7fe6136d";
            var WriterRoleId = "ff66713b-a71b-4d75-9da5-417900b1b4df";

            var roles = new List<IdentityRole>
            {
                new IdentityRole(){
                    Id = ReaderRoleId,
                    Name = "Reader",
                    NormalizedName = "Reader".ToUpper(),
                    ConcurrencyStamp = ReaderRoleId
                },
                new IdentityRole(){
                    Id = WriterRoleId,
                    Name = "Writer",
                    NormalizedName = "Writer".ToUpper(),
                    ConcurrencyStamp = WriterRoleId
                }
            };
            // Seed the roles
            builder.Entity<IdentityRole>().HasData(roles);

            // Create an Admin User
            var adminId = "f6f40cbd-ac64-41f1-88a2-2c87aa2d48a7";

            var admin = new IdentityUser()
            {
                Id = adminId,
                UserName = "admin@codepulse.com",
                Email = "admin@codepulse.com",
                NormalizedEmail = "admin@codepulse.com".ToUpper(),
                NormalizedUserName = "admin@codepulse.com".ToUpper()
            };

            // Seed the admin
            builder.Entity<IdentityUser>().HasData(admin);

            // Give Roles To Admin
            var adminRoles = new List<IdentityUserRole<string>>
            {
                new()
                {
                    UserId = adminId,
                    RoleId = ReaderRoleId
                },
                new()
                {
                    UserId = adminId,
                    RoleId = WriterRoleId
                }
            };
            builder.Entity<IdentityUserRole<string>>().HasData(adminRoles);

        }
    }
}

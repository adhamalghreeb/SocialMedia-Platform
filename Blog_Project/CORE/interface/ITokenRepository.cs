using Microsoft.AspNetCore.Identity;

namespace Blog_Project.CORE.@interface
{
    public interface ITokenRepository
    {
        string CreateJwtTok(IdentityUser identityusre, List<string> roles);
    }
}

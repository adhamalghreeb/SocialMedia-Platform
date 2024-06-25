using Microsoft.AspNetCore.Identity;

namespace Blog_Project.Repositories.Interface
{
    public interface ITokenRepository
    {
        string CreateJwtTok(IdentityUser identityusre, List<string>roles);
    }
}

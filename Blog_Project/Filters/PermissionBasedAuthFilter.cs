using Blog_Project.Attributes;
using Blog_Project.Data;
using Blog_Project.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace Blog_Project.Filters
{
    public class PermissionBasedAuthFilter : IAuthorizationFilter
    {
        private readonly appDBcontext appDBcontext;

        public PermissionBasedAuthFilter(appDBcontext appDBcontext)
        {
            this.appDBcontext = appDBcontext;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var attribute = (CheckPermAttribute)context.ActionDescriptor.EndpointMetadata.FirstOrDefault(x => x is CheckPermAttribute);
            if (attribute != null)
            {
                var claimIdentity = context.HttpContext.User.Identity as ClaimsIdentity;
                if (claimIdentity == null || !claimIdentity.IsAuthenticated)
                {
                    context.Result = new ForbidResult();
                }
                else
                {
                    var userId = claimIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
                    var hasPerm = appDBcontext.Set<UserPermissions>().Any(x => x.UserId == userId && x.PermissionId == attribute.permission);
                    if (!hasPerm)
                    {
                        context.Result = new ForbidResult();
                    }
                }
            }
            
        }
    }
}

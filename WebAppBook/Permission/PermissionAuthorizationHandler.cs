using Domain.Entity;
using Microsoft.AspNetCore.Authorization;

namespace WebAppBook.Permission
{
    public class PermissionAuthorizationHandler: AuthorizationHandler<PermissionRequirement>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (context.User == null)
            {
                return;
            }
            var Permission = context.User.Claims.Where(x =>
                                                              x.Type == Helper.Permission &&
                                                              x.Value == requirement.Permission &&
                                                              x.Issuer == "LOCAL AUTHORITY"
                                                        );
            if (Permission.Any())
            {
                context.Succeed(requirement);
                return;
            }
        }
    }
}

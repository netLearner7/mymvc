using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Heavy.Web.Auth
{
    public class zyz2Handler : AuthorizationHandler<userQueirment>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, userQueirment requirement)
        {
            if (context.User.IsInRole("zyz2")) {
                 context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}

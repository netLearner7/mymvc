using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Heavy.Web.Auth
{
    public class userQueirmentHandler : AuthorizationHandler<userQueirment>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, userQueirment requirement)
        {
            if (context.User.HasClaim(x => x.Type == "edit"))
             context.Succeed(requirement);

            return Task.CompletedTask;
        }
        
    }
}

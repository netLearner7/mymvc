using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Heavy.Web.Auth
{
    public class EmailHandler:AuthorizationHandler<qqMailRequirement>
    {

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, qqMailRequirement requirement)
        {
            var cliam = context.User.Claims.FirstOrDefault(x=>x.Type=="email");
      
            if (cliam!=null)
            {
                if (cliam.Value.EndsWith(requirement.requireEmail))
                {
                    context.Succeed(requirement);
                }

            }
            return Task.CompletedTask;

        }

    }
}

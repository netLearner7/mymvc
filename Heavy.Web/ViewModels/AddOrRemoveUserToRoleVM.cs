using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Heavy.Web.ViewModels
{
    public class AddOrRemoveUserToRoleVM
    {
        public AddOrRemoveUserToRoleVM()
        {
            identityUsers = new List<IdentityUser>();
        }

        public string RoleId { get; set; }

        public string UserId { get; set; }

        public List<IdentityUser> identityUsers { get; set; }
    }
}

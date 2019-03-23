using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Heavy.Web.Auth
{
    public class qqMailRequirement:IAuthorizationRequirement
    {
        public string requireEmail { get; set; }

        public qqMailRequirement(string email)
        {
            requireEmail = email;
        }
    }
}

using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Heavy.Web.Models
{
    public class ApplicationUser:IdentityUser
    {
        public ApplicationUser()
        {
            Claims = new List<IdentityUserClaim<string>>();
        }

            [MaxLength(18)]
            public string IdCardNo { get; set; }

            [DataType(DataType.Date)]
            public DateTime BirthDate { get; set; }

            public ICollection<IdentityUserClaim<string>> Claims { get; set; }
        
    }
}

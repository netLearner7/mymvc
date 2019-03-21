using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Heavy.Web.ViewModels
{
    public class RoleViewModel
    {
        [Required]
        [Display(Name = "角色名称")]
        public string name { get; set; }
    }
}

using Microsoft.AspNetCore.Mvc;
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
        [Remote("hhh","Role",ErrorMessage ="用户名有问题！")]
        public string name { get; set; }
    }
}

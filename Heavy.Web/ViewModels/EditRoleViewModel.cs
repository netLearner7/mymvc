using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Heavy.Web.ViewModels
{
    public class EditRoleViewModel
    {
        public string Id { get; set; }

        [Required]
        [Display(Name ="角色名称")]
        public string name { get; set; }

        public List<string> users { get; set; }
    }
}

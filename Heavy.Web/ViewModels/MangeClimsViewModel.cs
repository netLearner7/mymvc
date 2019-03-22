using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Heavy.Web.ViewModels
{
    public class MangeClimsViewModel
    {
        public string UserId { get; set; }

        public string ClimsId { get; set; }

        public List<string> AllClims { get; set;  }
    }
}

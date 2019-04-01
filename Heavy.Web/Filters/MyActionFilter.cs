using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Heavy.Web.Filters
{
    public class MyActionFilter : Attribute, IActionFilter
    {
        public string str { get; set; }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine("这里是{0}运行后filter", str);
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine("这里是{0}运行前filter", str);
        }
    }
}

using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Heavy.Web.Filters
{
    public class LogResourceFilter : Attribute, IResourceFilter
    {
        public string str { get; set; }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            Console.WriteLine("这里是{0}运行后filter", str);
        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            Console.WriteLine("这里是{0}运行前filter", str);
        }
    }
}

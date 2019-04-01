using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Heavy.Web.Filters
{
    public class LogAsyncResourceFilter :Attribute ,IAsyncResourceFilter
    {
        public string str { get; set; }
        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            
            Console.WriteLine("这里是{0}异步模式之前",str);

            var zhong =await next();

            Console.WriteLine("这里是{0}异步模式之后",str);

        }
    }
}

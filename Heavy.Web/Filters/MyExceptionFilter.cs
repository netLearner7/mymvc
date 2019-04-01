using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Heavy.Web.Filters
{
    public class MyExceptionFilter : Attribute, IExceptionFilter
    {
        public string str { get; set; }

        public void OnException(ExceptionContext context)
        {
            Console.WriteLine("这里是{0}运行filter", str);
        }
    }
}

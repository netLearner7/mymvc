using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Heavy.Web.Models;
using Microsoft.Extensions.Logging;
using Heavy.Web.Data;
using Heavy.Web.Filters;
using Microsoft.Extensions.Caching.Memory;

namespace Heavy.Web.Controllers
{
    [MyExceptionFilter(str = "Exception")]
    [MyResultFilter(str ="result")]
    [MyActionFilter(str = "Action")]
    [IdAuthorization(str = "Authorization")]
    [LogResourceFilter(str= "Resource")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly IMemoryCache memoryCache;

        public HomeController(ILogger<HomeController> logger,IMemoryCache memoryCache)
        {
            this.logger = logger;
            this.memoryCache = memoryCache;
        }
        
        //[ResponseCache(Duration = 30,Location =ResponseCacheLocation.Client]
        [ResponseCache(CacheProfileName ="defalut")]
        public IActionResult Index()
        {
            int i = 200;
            //logger.LogInformation(MyLog.log,"这里是str...............................................");
            logger.LogInformation(MyLog.log, "这里是str...............................................{0}",i);
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

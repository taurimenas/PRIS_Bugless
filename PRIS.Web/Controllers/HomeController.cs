using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PRIS.Web.Models;

namespace PRIS.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly string _user;

        public HomeController(ILogger<HomeController> logger, Microsoft.AspNetCore.Http.IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _user = httpContextAccessor.HttpContext.User.FindFirst(System.Security.Claims.ClaimTypes.Name).Value;
        }

        public IActionResult Index()
        {
            _logger.LogInformation("At {Time}. User {User} logged in", DateTime.UtcNow, _user);
            return View();
        }
        public IActionResult Main()
        {
            _logger.LogInformation("At {Time}. User {User} open this page.", DateTime.UtcNow, _user);
            return View();
        }
     
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Company.CRUD.MVC.PL.Models;
using Company.CRUD.MVC.PL.Services;
using System.Diagnostics;
using System.Text;

namespace Company.CRUD.MVC.PL.Controllers
{
	//[AllowAnonymous] // Default
	[Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IScopedService _scope01;
        private readonly IScopedService _scope02;
        private readonly ISingletonService _singleton01;
        private readonly ISingletonService _singleton02;
        private readonly ITransientService _transient01;
        private readonly ITransientService _transient02;

        public HomeController(
            ILogger<HomeController> logger,
            IScopedService scope01,
            IScopedService scope02,
            ISingletonService singleton01,
            ISingletonService singleton02,
            ITransientService transient01,
            ITransientService transient02

            )
        {
            _logger = logger;
            _scope01 = scope01;
            _scope02 = scope02;
            _singleton01 = singleton01;
            _singleton02 = singleton02;
            _transient01 = transient01;
            _transient02 = transient02;
        }

        // Home/TestLifeTime
        public string TestLifeTime()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append($"scope01 :: {_scope01.GetGuid()}\n");
            builder.Append($"scope02 :: {_scope02.GetGuid()}\n\n");

            builder.Append($"transient01 :: {_transient01.GetGuid()}\n");
            builder.Append($"transient02 :: {_transient02.GetGuid()}\n\n");

            builder.Append($"singleton01 :: {_singleton01.GetGuid()}\n");
            builder.Append($"singleton02 :: {_singleton02.GetGuid()}\n\n");

            return builder.ToString();
        }

        public IActionResult Index()
        {
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

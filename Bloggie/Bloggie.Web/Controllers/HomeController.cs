using Bloggie.Web.Models;
using Bloggie.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;
using System.Text.Json.Nodes;

namespace Bloggie.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
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

        public IActionResult AboutUs()
        {
            return View();
        }

        public IActionResult DashBoard()
        {
            return View();
        }

        public IActionResult Terms()
        {
            return View();
        }

        [HttpGet]

        public IActionResult getClient()
        {
             var webClient = new WebClient();
            var json = webClient.DownloadString(@"C:\Users\verit\Downloads\resjson.json");
            var data = JsonConvert.DeserializeObject<Root>(json);            
            return Json(data);

        }
      
    }
}
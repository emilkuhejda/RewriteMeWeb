using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RewriteMe.WebApi.Models;

namespace RewriteMe.WebApi.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ControlPanelController : Controller
    {
        [HttpGet("/control-panel")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("/control-panel/privacy")]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpGet("/control-panel/error")]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

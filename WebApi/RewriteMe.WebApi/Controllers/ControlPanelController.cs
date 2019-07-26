using Microsoft.AspNetCore.Mvc;

namespace RewriteMe.WebApi.Controllers
{
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
    }
}

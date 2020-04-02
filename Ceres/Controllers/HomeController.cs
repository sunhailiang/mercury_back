using Ceres.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Ceres.Controllers
{
    public class HomeController : Controller
    {
        [Route("/{constitution=pingheCons}")]
        public IActionResult Index(Constitution constitution)
        {
            return View("ModelRecipe", new UserConstitution { Constitution = constitution });
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

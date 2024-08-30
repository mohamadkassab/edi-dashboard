using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EDI.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public IActionResult Main()
        {
            ViewData["Title"] = "EDI";
            return View();
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Site.Controllers
{
    public class SupportController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
    }
}

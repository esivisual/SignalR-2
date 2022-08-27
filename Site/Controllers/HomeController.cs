using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Site.Context;
using Site.Models;
using Site.Models.Entites;
using System.Diagnostics;
using System.Security.Claims;

namespace Site.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DataBaseContext _context;
        public HomeController(ILogger<HomeController> logger, DataBaseContext context)
        {
            _logger = logger;
            _context = context;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(User user)
        {
            var findUser = _context.Users.Where(p => p.UserName == user.UserName
                && p.Password == user.Password).SingleOrDefault();

            if (findUser != null)
            {
                var Cliams = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,findUser.UserName),
                    new Claim(ClaimTypes.NameIdentifier, findUser.Id.ToString() ),
                };
                var identity = new ClaimsIdentity(Cliams, CookieAuthenticationDefaults.AuthenticationScheme);
                var properties = new AuthenticationProperties
                {
                    RedirectUri=Url.Content("/Support"),
                };
                return SignIn
                    (new ClaimsPrincipal(identity),
                    properties, CookieAuthenticationDefaults.AuthenticationScheme);
            }
            return View();
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
using Authenticate.Models.SignUp;
using Microsoft.AspNetCore.Mvc;

namespace Authenticate.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(RegisterUser model)
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }
    }
}

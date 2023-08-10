using Authenticate.Models;
using Authenticate.Models.SignUp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Authenticate.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public HomeController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult Register()
        {

            return View();
        }

        [Authorize]
        [Authorize(Roles = "Admin")]
        public IActionResult Privacy()
        {

            return View();
        }

        public IActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(UserViewModel model)
        {
            var user = await _userManager.FindByNameAsync(model.username);

            try
            {
                if (user != null)
                {
                    var signInResult = await _signInManager.PasswordSignInAsync(user, model.password, false, false);

                    if (signInResult.Succeeded)
                    {
                        TempData["Message"] = "Logged In";

                        // Check if the "Admin" role exists, if not, create it
                        if (!await _roleManager.RoleExistsAsync("Admin"))
                        {
                            await _roleManager.CreateAsync(new IdentityRole("Admin"));
                        }

                        // Check if the user is not already in the "Admin" role, then add it
                        if (!await _userManager.IsInRoleAsync(user, "Admin"))
                        {
                            await _userManager.AddToRoleAsync(user, "Admin");
                        }

                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch (Exception)
            {
                TempData["Message"] = "User doesn't Exist";
                return RedirectToAction(nameof(Index));
            }

            TempData["Message"] = "Login Failed";
            return RedirectToAction(nameof(Index));
        }

        // Other actions...
    

    [HttpPost]
    public async Task<IActionResult> Register(UserViewModel model)
    {
        if (ModelState.IsValid)
        {
            var userinfo = new IdentityUser
            {
                UserName = model.username,
                Email = string.Empty,
            };

            var userResult = await _userManager.CreateAsync(userinfo, model.password);

            if (userResult.Succeeded)
            {
                // Check if the "Admin" role exists, if not, create it
                if (!await _roleManager.RoleExistsAsync("Admin"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                }

                // Add the "Admin" role to the newly registered user
                await _userManager.AddToRoleAsync(userinfo, "Admin");

                var signInResult = await _signInManager.PasswordSignInAsync(userinfo, model.password, false, false);

                if (signInResult.Succeeded)
                {
                    TempData["Message"] = "User Registered";
                    return RedirectToAction(nameof(Index));
                }
            }
            TempData["Message"] = "User Registration Failed";
            return RedirectToAction(nameof(Index));
        }

        return View(model);
    }


        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
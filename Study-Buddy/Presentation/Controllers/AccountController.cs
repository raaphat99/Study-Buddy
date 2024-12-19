using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Presentation.ViewModels;

namespace Presentation.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel registerVM)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            ApplicationUser newUser = new ApplicationUser
            {
                UserName = registerVM.Username,
                Email = registerVM.Email,
                PasswordHash = registerVM.Password
            };

            IdentityResult result = await _userManager.CreateAsync(newUser, registerVM.Password);

            if (result.Succeeded)
            {
                // Register the user and create a cookie for them 
                await _signInManager.SignInAsync(newUser, false);
                return RedirectToAction("Index", "Home");

            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("Registration Error", error.Description);
                }
            }

            return View(registerVM);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var user = await _userManager.FindByEmailAsync(loginVM.Email);

            if (user == null)
            {
                ModelState.AddModelError("Login Error", "No user was found associated with the entered email address.");
            }
            else
            {
                bool success = await _userManager.CheckPasswordAsync(user, loginVM.Password);
                if (!success)
                {
                    ModelState.AddModelError("Login Error", "Incorrect password.");
                }
                else
                {
                    // Log the user in and create a cookie
                    await _signInManager.SignInAsync(user, loginVM.RememberMe);
                    return RedirectToAction("Index", "Home");
                }

            }

            return View(loginVM);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}

using GymSystem.BLL.ViewModels.AccountViewModels;
using GymSystem.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<AccountController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(SignInManager<ApplicationUser> signInManager , ILogger<AccountController> logger, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _logger = logger;
            _userManager = userManager;
        }

        #region LogIn

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult>LogIn(LogInViewModel model , CancellationToken ct = default )
        {
            if(!ModelState.IsValid) return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);

            if(user is null || string.IsNullOrEmpty(user.UserName))
            {
                ModelState.AddModelError(string.Empty, "Invalid Email Or Password");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, lockoutOnFailure: true);
            if (result.Succeeded) {

                _logger.LogInformation("User{UserId} SignedIn", user.Id);
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            if (result.IsLockedOut) {

                _logger.LogWarning("User {UserId} is locked out", user.Id);
                ModelState.AddModelError(string.Empty, "This account is temperory locked");
            
            }
            else if (result.IsNotAllowed)
            {
                ModelState.AddModelError(string.Empty, "This account is not allowed to sign in");

            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid Email or password");

            }
            return View(model);
        }
        #endregion

        #region LogOut
        [HttpPost]
        [Authorize]
        public async Task<IActionResult>Logout()
        {

            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(LogIn));
        }
        #endregion

        #region AccessDenied

        public IActionResult AccessDenied() => View();
        #endregion
    }
}

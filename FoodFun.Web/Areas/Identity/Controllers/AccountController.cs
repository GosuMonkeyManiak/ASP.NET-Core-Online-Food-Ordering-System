namespace FoodFun.Web.Areas.Identity.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using FoodFun.Infrastructure.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Models;

    using static Constants.GlobalConstants.Redirect;
    using static Constants.GlobalConstants.Roles;
    using static Constants.GlobalConstants.Areas;
    using static Constants.GlobalConstants.Messages;

    [Area(Identity)]
    public class AccountController : Controller
    {
        private readonly SignInManager<User> signInManger;
        private readonly UserManager<User> userManager;

        public AccountController(
            SignInManager<User> singInManager,
            UserManager<User> userManager)
        {
            this.signInManger = singInManager;
            this.userManager = userManager;
        }

        public IActionResult Register()
            => this.User.Identity.IsAuthenticated ? 
                RedirectToAction(Index, Home, new { area= "" }) :
                View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterFormModel formModel)
        {
            if (!this.ModelState.IsValid)
            {
                return View();
            }

            var isUserWithThatUsernameExist = await this.userManager.FindByNameAsync(formModel.Username) != null;

            if (isUserWithThatUsernameExist)
            {
                this.ViewData[nameof(AccountExist)] = AccountExist;

                return View();
            }

            var user = new User()
            {
                UserName = formModel.Username,
                Email = formModel.Email,
            };

            var identityResult = await this.userManager.CreateAsync(user, formModel.Password);

            if (!identityResult.Succeeded)
            {
                foreach (var error in identityResult.Errors)
                {
                    this.ModelState.AddModelError(string.Empty, error.Description);
                }

                return View();
            }

            await this.userManager.AddToRoleAsync(user, Customer);

            return RedirectToAction(nameof(Login));
        }
        
        public IActionResult Login(string returnUrl = null)
            => this.User.Identity.IsAuthenticated ?
                RedirectToAction(Index, Home, new { area = "" }) :
                View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginFormModel formModel, string returnUrl = null)
        {
            var loginResult = await this.signInManger
                .PasswordSignInAsync(
                    formModel.Username,
                    formModel.Password,
                    formModel.RememberMe,
                    true);

            if (loginResult.Succeeded)
            {
                return returnUrl == null ?
                    RedirectToAction(Index, Home, new { area = "" }) : 
                    Redirect(returnUrl);
            }
            else if (loginResult.IsLockedOut)
            {
                this.ModelState.AddModelError(string.Empty, AccountLockOut);

                return View();
            }

            this.ModelState.AddModelError(string.Empty, InvalidCredentials);

            return View();
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await this.signInManger.SignOutAsync();

            return RedirectToAction(Index, Home, new { area = "" });
        }

        [Authorize]
        public IActionResult AccessDenied()
            => View();
    }
}

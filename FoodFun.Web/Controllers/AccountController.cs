namespace FoodFun.Web.Controllers
{
    using Constants;
    using Microsoft.AspNetCore.Mvc;
    using Models.Account;
    using FoodFun.Infrastructure.Data.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;

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
                Redirect(GlobalConstants.Redirect.HomeIndexUrl) :
                View();

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Register(RegisterFormModel formModel)
        {
            if (!this.ModelState.IsValid)
            {
                return View();
            }

            var isUserWithThatUsernameExist = await this.userManager.FindByNameAsync(formModel.Username) != null;

            if (isUserWithThatUsernameExist)
            {
                this.ViewData[nameof(GlobalConstants.Messages.AccountExist)] = GlobalConstants.Messages.AccountExist;

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

            return RedirectToAction(nameof(Login));
        }
        
        public IActionResult Login(string returnUrl = null)
            => this.User.Identity.IsAuthenticated ?
                Redirect(GlobalConstants.Redirect.HomeIndexUrl) :
                View();

        [HttpPost]
        [AutoValidateAntiforgeryToken]
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
                    Redirect(GlobalConstants.Redirect.HomeIndexUrl) : 
                    Redirect(returnUrl);
            }
            else if (loginResult.IsLockedOut)
            {
                this.ModelState.AddModelError(string.Empty, GlobalConstants.Messages.AccountLockOut);

                return View();
            }

            this.ModelState.AddModelError(string.Empty, GlobalConstants.Messages.InvalidCredentials);

            return View();
        }

        [Authorize]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Logout()
        {
            await this.signInManger.SignOutAsync();

            return Redirect(GlobalConstants.Redirect.HomeIndexUrl);
        }

        [Authorize]
        public IActionResult AccessDenied()
            => View();
    }
}

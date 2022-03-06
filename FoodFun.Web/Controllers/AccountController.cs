namespace FoodFun.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Models.Account;
    using FoodFun.Infrastructure.Data.Models;
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
            => View();

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
                this.ViewData["Message"] = "Account already exist!";

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

        public IActionResult Login()
            => View();

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login(LoginFormModel formModel)
        {
            var loginResult = await this.signInManger
                .PasswordSignInAsync(
                    formModel.Username,
                    formModel.Password,
                    formModel.RememberMe,
                    true);

            if (loginResult.Succeeded)
            {
                return Redirect("/Home/Index");
            }
            else if (loginResult.IsLockedOut)
            {
                this.ModelState.AddModelError(string.Empty, "Account locked out for one hour.Please try again later.");

                return View();
            }

            this.ModelState.AddModelError(string.Empty, "Invalid credentials.");

            return View();
        }
    }
}

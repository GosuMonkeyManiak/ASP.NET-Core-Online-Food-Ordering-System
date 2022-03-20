namespace FoodFun.Web.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Models.Role;
    using static Constants.GlobalConstants.Roles;
    using static Constants.GlobalConstants.Areas;

    [Area(Administration)]
    [Authorize(Roles = Administrator)]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager) 
            => this.roleManager = roleManager;

        public IActionResult Add()
            => View();

        [HttpPost]
        public async Task<IActionResult> Add(RoleFormModel formModel)
        {
            if (!this.ModelState.IsValid)
            {
                return View();
            }

            await this.roleManager.CreateAsync(new IdentityRole(formModel.Title));

            return RedirectToAction(nameof(Add));
        }
    }
}

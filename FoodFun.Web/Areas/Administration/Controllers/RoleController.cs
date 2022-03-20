namespace FoodFun.Web.Areas.Administration.Controllers
{
    using Core.Extensions;
    using global::AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Models.Role;
    using static Constants.GlobalConstants.Areas;
    using static Constants.GlobalConstants.Roles;

    [Area(Administration)]
    [Authorize(Roles = Administrator)]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IMapper mapper;

        public RoleController(
            RoleManager<IdentityRole> roleManager, 
            IMapper mapper)
        {
            this.roleManager = roleManager;
            this.mapper = mapper;
        }

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

            return RedirectToAction(nameof(All));
        }

        public async Task<IActionResult> All()
        {
            var roles = await this.roleManager
                .Roles
                .AsNoTracking()
                .ToListAsync();

            return View(roles.ProjectTo<RoleListingModel>(this.mapper));
        }

        public async Task<IActionResult> Edit()
            => View();
    }
}

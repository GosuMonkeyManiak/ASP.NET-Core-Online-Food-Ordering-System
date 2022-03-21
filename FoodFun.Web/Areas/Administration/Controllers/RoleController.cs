namespace FoodFun.Web.Areas.Administration.Controllers
{
    using Core.Extensions;
    using global::AutoMapper;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Models.Role;

    using static Constants.GlobalConstants.Messages;

    public class RoleController : AdminBaseController
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

        public async Task<IActionResult> Edit(string id)
        {
            var role = await this.roleManager.FindByIdAsync(id);

            if (role == null)
            {
                this.TempData[Error] = RoleNotExit;

                return RedirectToAction(nameof(All));
            }
            
            return View(new RoleEditModel()
            {
                Id = role.Id,
                Title = role.Name
            });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RoleEditModel editModel)
        {
            if (!this.ModelState.IsValid)
            {
                return View(editModel);
            }

            var role = await this.roleManager.FindByIdAsync(editModel.Id);

            if (role == null)
            {
                this.TempData[Error] = RoleNotExit;

                return RedirectToAction(nameof(All));
            }

            role.Name = editModel.Title;

            await this.roleManager.UpdateAsync(role);

            return RedirectToAction(nameof(All));
        }

        public async Task<IActionResult> Delete(string id)
        {
            var role = await this.roleManager.FindByIdAsync(id);

            if (role == null)
            {
                this.TempData[Error] = RoleNotExit;

                return RedirectToAction(nameof(All));
            }

            await this.roleManager.DeleteAsync(role);

            return RedirectToAction(nameof(All));
        }
    }
}

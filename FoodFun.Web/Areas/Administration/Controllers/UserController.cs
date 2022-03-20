namespace FoodFun.Web.Areas.Administration.Controllers
{
    using Core.Extensions;
    using global::AutoMapper;
    using Infrastructure.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using Models.User;

    using static Constants.GlobalConstants.Roles;
    using static Constants.GlobalConstants.Areas;

    [Area(Administration)]
    [Authorize(Roles = Administrator)]
    public class UserController : Controller
    {
        private readonly IMapper mapper;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public UserController(
            IMapper mapper, 
            UserManager<User> userManager, 
            RoleManager<IdentityRole> roleManager)
        {
            this.mapper = mapper;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task<IActionResult> All(UserSearchModel searchModel)
        {
            var usersAsQuery = this.userManager
                .Users
                .AsQueryable();

            if (searchModel.SearchTerm != null)
            {
                usersAsQuery = usersAsQuery
                    .Where(x => x.UserName.Contains(searchModel.SearchTerm)
                                || x.Email.Contains(searchModel.SearchTerm));
            }

            var users = await usersAsQuery
                .ToListAsync();

            return View(new UserSearchModel()
            {
                Users = users.ProjectTo<UserListingModel>(this.mapper)
            });
        }

        public async Task<IActionResult> Details(string id)
        {
            var user = await this.userManager.FindByIdAsync(id);

            if (user == null)
            {
                //TODO: add error
                return RedirectToAction(nameof(All));
            }

            var allRoles = await this.roleManager.Roles.ToListAsync();

            var selectedRoles = new List<SelectListItem>(allRoles.Count);

            foreach (var role in allRoles)
            {
                var selectItem = new SelectListItem()
                {
                    Text = role.Name,
                    Value = role.Id,
                    Selected = await this.userManager.IsInRoleAsync(user, role.Name)
                };

                selectedRoles.Add(selectItem);
            }

            var userDetailModel = new UserDetailsModel()
            {
                Id = user.Id,
                Username = user.UserName,
                Roles = selectedRoles
            };

            return View(userDetailModel);
        }
    }
}

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
    using static Constants.GlobalConstants.Messages;

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
                this.TempData.Add(Error, UserNotExist);
                
                return RedirectToAction(nameof(All));
            }

            var allRoles = await this.roleManager.Roles.ToListAsync();

            var selectListItem = await CreateSelectListItemsForRoles(user, allRoles);

            var userDetailModel = new UserDetailsModel()
            {
                Id = user.Id,
                Username = user.UserName,
                Roles = selectListItem.ToList()
            };

            return View(userDetailModel);
        }

        [HttpPost]
        public async Task<IActionResult> Details(UserDetailsModel detailsModel)
        {
            if (!ModelState.IsValid)
            {
                return View(detailsModel);
            }

            var user = await this.userManager.FindByIdAsync(detailsModel.Id);

            if (user == null)
            {
                this.TempData[Error] = UserNotExist;

                return RedirectToAction(nameof(All));
            }

            await ChangeRolesForUser(detailsModel, user);

            return RedirectToAction(nameof(All));
        }

        private async Task<IEnumerable<SelectListItem>> CreateSelectListItemsForRoles(
            User user,
            IEnumerable<IdentityRole> allRoles)
        {
            var selectedRoles = new List<SelectListItem>(allRoles.Count());

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

            return selectedRoles;
        }

        private async Task ChangeRolesForUser(
            UserDetailsModel detailsModel,
            User user)
        {
            foreach (var item in detailsModel.Roles)
            {
                var role = await this.roleManager.FindByIdAsync(item.Value);

                if (role != null)
                {
                    var isInRoleUser = await this.userManager.IsInRoleAsync(user, role.Name);

                    if (item.Selected)
                    {
                        if (!isInRoleUser)
                        {
                            await this.userManager.AddToRoleAsync(user, role.Name);
                        }
                    }
                    else if (isInRoleUser)
                    {
                        await this.userManager.RemoveFromRoleAsync(user, role.Name);
                    }
                }
            }
        }
    }
}

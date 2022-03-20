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
        private readonly RoleManager<User> roleManager;

        public UserController(
            IMapper mapper, 
            UserManager<User> userManager)
        {
            this.mapper = mapper;
            this.userManager = userManager;
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

        //public async Task<IActionResult> Details(string id)
        //{

        //    //var userWithRoles = await this.userService.GetByIdWithRolesOrDefault(id);

        //    if (userWithRoles == null)
        //    {
        //        //TODO: Add alert to all for user not exist
        //        return RedirectToAction(nameof(All));
        //    }

        //    var selectedRoles = new List<SelectListItem>(userWithRoles.Roles.Count());

        //    foreach (var role in userWithRoles.Roles)
        //    {
        //        selectedRoles.Add(new()
        //        {
        //            Text = role.Title,
        //            Value = role.Id,
        //            Selected = true
        //        });
        //    }

        //    var userDetailModel = new UserDetailsModel()
        //    {
        //        Id = userWithRoles.Id,
        //        Username = userWithRoles.Username,
        //        Roles = selectedRoles
        //    };


        //    return View(userDetailModel);
        //}

        //[HttpPost]
        //public IActionResult Details(UserDetailsModel detailsModel)
        //{
        //    return null;
        //}
    }
}

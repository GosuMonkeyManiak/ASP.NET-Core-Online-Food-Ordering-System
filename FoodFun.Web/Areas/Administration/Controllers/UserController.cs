namespace FoodFun.Web.Areas.Administration.Controllers
{
    using Core.Contracts;
    using Core.Extensions;
    using global::AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Models.User;
    using static Constants.GlobalConstants.Roles;
    using static Constants.GlobalConstants.Areas;

    [Area(Administration)]
    [Authorize(Roles = Administrator)]
    public class UserController : Controller
    {
        private readonly IUserService userService;
        private readonly IMapper mapper;

        public UserController(
            IUserService userService, 
            IMapper mapper)
        {
            this.userService = userService;
            this.mapper = mapper;
        }

        public async Task<IActionResult> All(UserSearchModel searchModel)
        {
            var users = await this.userService.All(searchModel.SearchTerm);

            return View(new UserSearchModel()
            {
                Users = users.ProjectTo<UserListingModel>(this.mapper)
            });
        }
    }
}

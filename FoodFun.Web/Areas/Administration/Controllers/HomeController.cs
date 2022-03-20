namespace FoodFun.Web.Areas.Administration.Controllers
{
    using Infrastructure.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using static Constants.GlobalConstants.Roles;
    using static Constants.GlobalConstants.Areas;

    [Area(Administration)]
    [Authorize(Roles = Administrator)]
    public class HomeController : Controller
    {
        private readonly UserManager<User> userManager;

        public HomeController(UserManager<User> userManager) 
            => this.userManager = userManager;

        public async Task<IActionResult> Index() 
            => View((await this.userManager
                .Users
                .AsNoTracking()
                .ToListAsync()).Count);
    }
}

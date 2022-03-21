namespace FoodFun.Web.Areas.Administration.Controllers
{
    using Infrastructure.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class HomeController : AdminBaseController
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

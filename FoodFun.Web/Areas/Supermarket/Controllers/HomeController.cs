namespace FoodFun.Web.Areas.Supermarket.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static Constants.GlobalConstants.Areas;
    using static Constants.GlobalConstants.Roles;

    [Area(Supermarket)]
    [Authorize(Roles = SupermarketManager)]
    public class HomeController : Controller
    {
        public IActionResult Index() 
            => View();
    }
}

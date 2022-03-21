namespace FoodFun.Web.Areas.Restaurant.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : RestaurantBaseController
    {
        public IActionResult Index()
            => View();
    }
}

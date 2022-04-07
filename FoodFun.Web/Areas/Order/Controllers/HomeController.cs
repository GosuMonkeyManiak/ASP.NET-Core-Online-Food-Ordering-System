namespace FoodFun.Web.Areas.Order.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : OrderBaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

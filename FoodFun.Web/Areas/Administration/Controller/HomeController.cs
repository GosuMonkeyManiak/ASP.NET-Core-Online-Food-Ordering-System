namespace FoodFun.Web.Areas.Administration.Controller
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static Constants.GlobalConstants.Roles;
    using static Constants.GlobalConstants.Areas;

    [Authorize(Roles = Administrator)]
    [Area(Administration)]
    public class HomeController : Controller
    {
        public IActionResult Index() 
            => View();
    }
}

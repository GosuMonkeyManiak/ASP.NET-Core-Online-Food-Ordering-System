namespace FoodFun.Web.Areas.Restaurant.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static Constants.GlobalConstants.Areas;
    using static Constants.GlobalConstants.Roles;

    [Area(Restaurant)]
    [Authorize(Roles = RestaurantManager)]
    public abstract class RestaurantBaseController : Controller
    {
    }
}

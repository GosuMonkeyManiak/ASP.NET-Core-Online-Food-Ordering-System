namespace FoodFun.Web.Areas.Order.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static Constants.GlobalConstants.Areas;
    using static Constants.GlobalConstants.Roles;

    [Area(Order)]
    [Authorize(Roles = OrderManager)]
    public abstract class OrderBaseController : Controller
    {

    }
}

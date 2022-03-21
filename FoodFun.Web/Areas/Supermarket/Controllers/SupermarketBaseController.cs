namespace FoodFun.Web.Areas.Supermarket.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static Constants.GlobalConstants.Roles;
    using static Constants.GlobalConstants.Areas;

    [Area(Supermarket)]
    [Authorize(Roles = SupermarketManager)]
    public abstract class SupermarketBaseController : Controller
    {
    }
}

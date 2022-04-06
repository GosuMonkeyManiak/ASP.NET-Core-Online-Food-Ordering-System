namespace FoodFun.Web.Areas.Order.Controllers
{
    using Core.Contracts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models.Cart;

    using static Constants.GlobalConstants.Areas;

    [Area(Order)]
    public class OrderController : Controller
    {
        private readonly IOrderService orderService;

        public OrderController(IOrderService orderService) 
            => this.orderService = orderService;

        [HttpPost]
        [Authorize]
        public IActionResult Create(CartListingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return RedirectToAction("Cart", "Cart", new { area = "" });
            }

            return Ok(model);
        }
    }
}

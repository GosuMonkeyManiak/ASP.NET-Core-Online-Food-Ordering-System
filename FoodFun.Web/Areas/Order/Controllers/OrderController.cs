namespace FoodFun.Web.Areas.Order.Controllers
{
    using Core.Contracts;
    using FoodFun.Core.Extensions;
    using FoodFun.Core.Models.Cart;
    using global::AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models.Cart;
    using System.Security.Claims;

    using static Constants.GlobalConstants.Areas;
    using static Constants.GlobalConstants.Messages;

    [Area(Order)]
    public class OrderController : Controller
    {
        private readonly IOrderService orderService;
        private readonly IMapper mapper;

        public OrderController(
            IOrderService orderService,
            IMapper mapper)
        {
            this.orderService = orderService;
            this.mapper = mapper;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(CartListingModel cartItems)
        {
            if (!this.ModelState.IsValid)
            {
                this.TempData[Error] = SomethingWentWrong;

                return RedirectToAction("Cart", "Cart", new { area = "" });
            }

            var userId = this.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;

            var orderNumber = await this.orderService
                .Create(
                    userId, 
                    cartItems.Products.ProjectTo<CartItemModel>(this.mapper),
                    cartItems.Dishes.ProjectTo<CartItemModel>(this.mapper));

            this.HttpContext.Session.Clear();

            return View(orderNumber);
        }
    }
}

namespace FoodFun.Web.Areas.Order.Controllers
{
    using FoodFun.Core.Contracts;
    using FoodFun.Core.Extensions;
    using FoodFun.Web.Areas.Order.Models.Order;
    using global::AutoMapper;
    using Microsoft.AspNetCore.Mvc;

    public class OrderController : OrderBaseController
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

        public async Task<IActionResult> All()
        {
            var orders = await this.orderService.All();

            return View(orders.ProjectTo<OrderListingModel>(this.mapper));
        }

        //public async Task<IActionResult> Details(int id)
        //{

        //}
    }
}

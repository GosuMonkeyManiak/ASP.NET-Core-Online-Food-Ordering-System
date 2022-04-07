namespace FoodFun.Web.Areas.Order.Controllers
{
    using FoodFun.Core.Contracts;
    using FoodFun.Core.Extensions;
    using FoodFun.Web.Areas.Order.Models.Order;
    using global::AutoMapper;
    using Microsoft.AspNetCore.Mvc;

    using static Constants.GlobalConstants.Messages;

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

        public async Task<IActionResult> Details(int id)
        {
            if (!await this.orderService.IsOrderExist(id))
            {
                this.TempData[Error] = OrderNotExist;

                return RedirectToAction(nameof(All));
            }

            var orderWithItems = await this.orderService.ByIdWithItems(id);

            return View(this.mapper.Map<OrderWithItemsListingModel>(orderWithItems));
        }

        public async Task<IActionResult> Sent(int id)
        {
            if (!await this.orderService.IsOrderExist(id))
            {
                this.TempData[Error] = OrderNotExist;

                return RedirectToAction(nameof(All));
            }

            await this.orderService.Sent(id);

            return RedirectToAction(nameof(All));
        }

        public async Task<IActionResult> Delivered(int id)
        {
            if (!await this.orderService.IsOrderExist(id))
            {
                this.TempData[Error] = OrderNotExist;

                return RedirectToAction(nameof(All));
            }

            await this.orderService.Deliver(id);

            return RedirectToAction(nameof(All));
        }
    }
}

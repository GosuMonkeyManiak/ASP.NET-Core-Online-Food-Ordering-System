namespace FoodFun.Core.Services
{
    using Contracts;
    using FoodFun.Core.Extensions;
    using FoodFun.Core.Models.Cart;
    using FoodFun.Core.Models.Order;
    using FoodFun.Infrastructure.Common.Contracts;
    using FoodFun.Infrastructure.Models;
    using global::AutoMapper;
    using System.Threading.Tasks;

    public class OrderService : IOrderService
    {
        private readonly IOrderRepository orderRepository;
        private readonly IMapper mapper;
        private readonly IProductService productService;
        private readonly IDishService dishService;

        public OrderService(
            IOrderRepository orderRepository,
            IMapper mapper,
            IProductService productService,
            IDishService dishService)
        { 
            this.orderRepository = orderRepository;
            this.mapper = mapper;
            this.productService = productService;
            this.dishService = dishService;
        }

        public async Task<IEnumerable<OrderServiceModel>> All()
        {
            var orders = await this.orderRepository.AllWithUsers();

            return orders.ProjectTo<OrderServiceModel>(this.mapper);
        }

        public async Task<int> Create(
            string userId, 
            IEnumerable<CartItemModel> products,
            IEnumerable<CartItemModel> dishes)
        {
            var productsPrice = await this.productService.PriceForProducts(products.Select(x => x.Id).ToArray());
            var dishesPrice = await this.dishService.PriceForDishes(dishes.Select(x => x.Id).ToArray());

            var order = new Order()
            {
                UserId = userId,
                OrderProducts = products.ProjectTo<OrderProduct>(this.mapper).ToList(),
                OrderDishes = dishes.ProjectTo<OrderDish>(this.mapper).ToList(),
                Price = productsPrice + dishesPrice
            };

            await this.orderRepository
                .AddAsync(order);

            await this.orderRepository
                .SaveChangesAsync();

            return order.Id;
        }
    }
}

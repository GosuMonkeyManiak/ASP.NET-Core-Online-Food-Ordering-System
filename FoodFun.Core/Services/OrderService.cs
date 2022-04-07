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
        private readonly IDishService dishService;
        private readonly IMapper mapper;
        private readonly IOrderRepository orderRepository;
        private readonly IProductService productService;

        public OrderService(
             IDishService dishService,
             IMapper mapper,
             IOrderRepository orderRepository,
             IProductService productService)
        { 
            this.dishService = dishService;
            this.mapper = mapper;
            this.orderRepository = orderRepository;
            this.productService = productService;
        }

        public async Task<IEnumerable<OrderServiceModel>> All(bool onlyActive = true)
        {
            var orders = await this.orderRepository.AllWithUsers(onlyActive);

            return orders.ProjectTo<OrderServiceModel>(this.mapper);
        }

        public async Task<OrderWithItemsServiceModel> ByIdWithItems(int id)
        {
            var order = await this.orderRepository.ByItWithItems(id);

            var products = (await this.productService.All(order.OrderProducts.Select(x => x.ProductId).ToArray()))
                .Select(x =>
                {
                    x.Quantity = order.OrderProducts.First(x => x.ProductId == x.ProductId).Quantity;
                    return x;
                });
            var dishes = (await this.dishService.All(order.OrderDishes.Select(x => x.DishId).ToArray()))
                .Select(x =>
                {
                    x.Quantity = order.OrderDishes.First(x => x.DishId == x.DishId).Quantity;
                    return x;
                });

            var orderWithItems = new OrderWithItemsServiceModel()
            {
                UserEmail = order.User.Email,
                Price = order.Price,
                Products = products,
                Dishes = dishes
            };

            return orderWithItems;
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

        public async Task Deliver(int id)
        {
            var order = await this.orderRepository.FindAsync(x => x.Id == id);
            order.IsDelivered = true;

            this.orderRepository
                .Update(order);

            await this.orderRepository
                .SaveChangesAsync();
        }

        public async Task<bool> IsOrderExist(int id)
            => (await this.orderRepository
                .FindOrDefaultAsync(x => x.Id == id)) != null;

        public async Task Sent(int id)
        {
            var order =  await this.orderRepository.FindAsync(x => x.Id == id);
            order.IsSent = true;

            this.orderRepository
                .Update(order);

            await this.orderRepository
                    .SaveChangesAsync();
        }
    }
}

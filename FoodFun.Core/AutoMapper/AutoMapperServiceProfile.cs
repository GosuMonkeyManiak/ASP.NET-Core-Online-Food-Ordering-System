namespace FoodFun.Core.AutoMapper
{
    using FoodFun.Core.Models.Cart;
    using FoodFun.Core.Models.Order;
    using global::AutoMapper;
    using Infrastructure.Models;
    using Models.Dish;
    using Models.DishCategory;
    using Models.Product;
    using Models.ProductCategory;

    public class AutoMapperServiceProfile : Profile
    {
        public AutoMapperServiceProfile()
        {
            CreateMap<Product, ProductServiceModel>();

            CreateMap<ProductCategory, ProductCategoryServiceModel>();
            CreateMap<ProductCategory, ProductCategoryWithProductCountServiceModel>()
                .ForMember(x => x.ProductsCount, options => options
                    .MapFrom(x => x.Products.Count));

           
            CreateMap<Dish, DishServiceModel>();
            CreateMap<DishCategory, DishCategoryServiceModel>();
            CreateMap<DishCategory, DishCategoryWithDishCountServiceModel>()
                .ForMember(x => x.Count, options => options
                    .MapFrom(x => x.Dishes.Count));

            CreateMap<CartItemModel, OrderProduct>()
                .ForMember(x => x.ProductId, options => options.MapFrom(x => x.Id));

            CreateMap<CartItemModel, OrderDish>()
                .ForMember(x => x.DishId, options => options.MapFrom(x => x.Id));

            CreateMap<Order, OrderServiceModel>()
                .ForMember(x => x.UserEmail, options => options.MapFrom(x => x.User.Email));
        }
    }
}

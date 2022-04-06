namespace FoodFun.Web.AutoMapper
{
    using Areas.Administration.Models;
    using Areas.Administration.Models.Role;
    using Areas.Restaurant.Models.Dish;
    using Areas.Restaurant.Models.DishCategory;
    using Areas.Supermarket.Models.Product;
    using Areas.Supermarket.Models.ProductCategory;
    using Core.Models.Dish;
    using Core.Models.DishCategory;
    using Core.Models.Product;
    using Core.Models.ProductCategory;
    using FoodFun.Core.Models.Cart;
    using global::AutoMapper;
    using Infrastructure.Models;
    using Microsoft.AspNetCore.Identity;
    using Models.Product;

    public class AutoMapperWebProfile : Profile
    {
        public AutoMapperWebProfile()
        {
            CreateMap<ProductServiceModel, ProductListingModel>()
                .ForMember(x => x.CategoryTitle, options => options
                    .MapFrom(x => x.Category.Title));

            CreateMap<ProductServiceModel, ProductEditModel>()
                .ForMember(x => x.CategoryId, options => options.
                    MapFrom(x => x.Category.Id));

            CreateMap<ProductCategoryServiceModel, ProductCategoryEditModel>();
            CreateMap<ProductCategoryWithProductCountServiceModel, ProductCategoryListingModel>();
            CreateMap<ProductCategoryServiceModel, ProductCategoryModel>();
            CreateMap<ProductListingModel, CartItemModel>();

            CreateMap<DishServiceModel, DishListingModel>()
                .ForMember(x => x.CategoryTitle, options => options
                    .MapFrom(x => x.Category.Title));

            CreateMap<DishServiceModel, DishEditModel>()
                .ForMember(x => x.CategoryId, options => options
                    .MapFrom(x => x.Category.Id));
            
            CreateMap<DishCategoryServiceModel, DishCategoryModel>();
            CreateMap<DishCategoryServiceModel, DishCategoryEditModel>();
            CreateMap<DishCategoryWithDishCountServiceModel, DishCategoryListingModel>();
            CreateMap<DishListingModel, CartItemModel>();

            CreateMap<User, UserListingModel>();

            CreateMap<IdentityRole, RoleListingModel>()
                .ForMember(x => x.Title, options => options
                    .MapFrom(x => x.Name));
        }
    }
}

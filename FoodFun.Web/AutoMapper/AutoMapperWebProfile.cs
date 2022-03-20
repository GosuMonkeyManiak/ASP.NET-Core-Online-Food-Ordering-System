﻿namespace FoodFun.Web.AutoMapper
{
    using Areas.Administration.Models;
    using Areas.Administration.Models.User;
    using Core.Models.Dish;
    using Core.Models.DishCategory;
    using Core.Models.Product;
    using Core.Models.ProductCategory;
    using global::AutoMapper;
    using Infrastructure.Models;
    using Models.Dish;
    using Models.DishCategory;
    using Models.Product;
    using Models.ProductCategory;

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

            CreateMap<DishServiceModel, DishListingModel>()
                .ForMember(x => x.CategoryTitle, options => options
                    .MapFrom(x => x.Category.Title));

            CreateMap<DishServiceModel, DishEditModel>()
                .ForMember(x => x.CategoryId, options => options
                    .MapFrom(x => x.Category.Id));

            CreateMap<DishCategoryServiceModel, DishCategoryModel>();
            CreateMap<DishCategoryServiceModel, DishCategoryEditModel>();
            CreateMap<DishCategoryWithDishCountServiceModel, DishCategoryListingModel>();

            CreateMap<User, UserListingModel>();
        }
    }
}

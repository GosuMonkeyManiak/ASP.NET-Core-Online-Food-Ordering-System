namespace FoodFun.Web.AutoMapper
{
    using Core.Models.Dish;
    using Core.Models.DishCategory;
    using Core.Models.Product;
    using Core.Models.ProductCategory;
    using global::AutoMapper;
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
            CreateMap<DishCategoryServiceModel, DishCategoryModel>();
            CreateMap<DishServiceModel, DishEditModel>()
                .ForMember(x => x.CategoryId, options => options
                    .MapFrom(x => x.Category.Id));
            CreateMap<DishCategoryWithDishCountServiceModel, DishCategoryListingModel>();
        }
    }
}

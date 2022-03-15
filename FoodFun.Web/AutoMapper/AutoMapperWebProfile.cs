namespace FoodFun.Web.AutoMapper
{
    using Core.Models.Product;
    using Core.Models.ProductCategory;
    using global::AutoMapper;
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
        }
    }
}

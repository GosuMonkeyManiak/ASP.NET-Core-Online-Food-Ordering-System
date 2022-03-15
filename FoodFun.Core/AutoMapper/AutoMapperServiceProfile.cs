namespace FoodFun.Core.AutoMapper
{
    using global::AutoMapper;
    using Infrastructure.Models;
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
        }
    }
}

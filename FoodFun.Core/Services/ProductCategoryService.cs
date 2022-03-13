namespace FoodFun.Core.Services
{
    using Contracts;
    using Infrastructure.Common.Contracts;
    using Infrastructure.Models;
    using Models.ProductCategory;

    public class ProductCategoryService : IProductCategoryService
    {
        private readonly IProductCategoryRepository productCategoryRepository;

        public ProductCategoryService(IProductCategoryRepository productCategoryRepository)
        {
            this.productCategoryRepository = productCategoryRepository;
        }

        public async Task Add(string title)
            => await this.productCategoryRepository
                .AddAsync(new() { Title = title });

        public async Task<IEnumerable<ProductCategoryWithProductCountServiceModel>> All()
        {
            var categoriesWithProducts = await this.productCategoryRepository
                .GetAllCategoriesWithProducts();

            return categoriesWithProducts
                .Select(pc => new ProductCategoryWithProductCountServiceModel()
                {
                    Id = pc.Id,
                    Title = pc.Title,
                    ProductsCount = pc.Products.Count
                });
        }

        public async Task<bool> IsCategoryExist(int id)
            => await this.productCategoryRepository
                .FindOrDefault(c => c.Id == id) != null;

        public async Task<Tuple<bool, ProductCategoryServiceModel>> GetById(int id)
        {
            if (!await IsCategoryExist(id))
            {
                return new(false, null);
            }

            var productCategory = await this.productCategoryRepository
                .FindOrDefault(x => x.Id == id);

            var productCategoryServiceModel = new ProductCategoryServiceModel()
            {
                Id = productCategory.Id,
                Title = productCategory.Title
            };

            return new(true, productCategoryServiceModel);
        }

        public async Task<bool> Update(int categoryId, string title)
        {
            if (!await IsCategoryExist(categoryId))
            {
                return false;
            }

            var productCategory = new ProductCategory()
            {
                Id = categoryId,
                Title = title
            };

            this.productCategoryRepository.Update(productCategory);

            await this.productCategoryRepository.SaveChangesAsync();

            return true;
        }
    }
}

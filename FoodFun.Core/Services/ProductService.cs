namespace FoodFun.Core.Services
{
    using Contracts;
    using Extensions;
    using global::AutoMapper;
    using Infrastructure.Common.Contracts;
    using Infrastructure.Models;
    using Models.Product;

    public class ProductService : IProductService
    {
        private readonly IProductRepository productRepository;
        private readonly IProductCategoryService productCategoryService;
        private readonly IMapper mapper;

        public ProductService(
            IProductRepository productRepository,
            IProductCategoryService productCategoryService,
            IMapper mapper)
        {
            this.productRepository = productRepository;
            this.productCategoryService = productCategoryService;
            this.mapper = mapper;
        }

        public async Task<bool> AddProduct(
            string name, 
            string imageUrl, 
            int categoryId, 
            decimal price, 
            string description)
        {
            if (!await this.productCategoryService.IsCategoryExist(categoryId))
            {
                return false;
            }

            var product = new Product()
            {
                Name = name,
                ImageUrl = imageUrl,
                CategoryId = categoryId,
                Price = price,
                Description = description
            };

            await this.productRepository
                .AddAsync(product);

            await this.productRepository
                .SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<ProductServiceModel>> All()
        {
            var productsWithCategories = await this.productRepository
                .GetAllProductsWithCategories();

            return productsWithCategories
                .ProjectTo<ProductServiceModel>(this.mapper);
        }

        public async Task<Tuple<bool, ProductServiceModel>> GetById(string id)
        {
            if (!await IsProductExist(id))
            {
                return new(false, null);
            }

            var product = await this.productRepository
                .GetProductWithCategoryById(id);

            return new(true, this.mapper.Map<ProductServiceModel>(product));
        }

        public async Task<bool> Update(
            string id, 
            string name, 
            string imageUrl, 
            int categoryId,
            decimal price, 
            string description)
        {
            if (!await this.productCategoryService.IsCategoryExist(categoryId)
                || !await IsProductExist(id))
            {
                return false;
            }

            var product = new Product()
            {
                Id = id,
                Name = name,
                ImageUrl = imageUrl,
                CategoryId = categoryId,
                Price = price,
                Description = description
            };

            this.productRepository.Update(product);

            await this.productRepository.SaveChangesAsync();

            return true;
        }

        private async Task<bool> IsProductExist(string productId)
            => await this.productRepository
                .FindOrDefault(p => p.Id == productId) != null;
    }
}

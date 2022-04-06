namespace FoodFun.Core.Services
{
    using Base;
    using Contracts;
    using Extensions;
    using global::AutoMapper;
    using Infrastructure.Common.Contracts;
    using Models.Product;

    public class ProductService : BaseItemService, IProductService
    {
        private readonly IMapper mapper;
        private readonly IProductCategoryService productCategoryService;
        private readonly IProductRepository productRepository;

        public ProductService(
            IProductRepository productRepository,
            IProductCategoryService productCategoryService,
            IMapper mapper)
        {
            this.productRepository = productRepository;
            this.productCategoryService = productCategoryService;
            this.mapper = mapper;
        }

        public async Task Add(
            string name, 
            string imageUrl, 
            int categoryId, 
            decimal price, 
            string description,
            ulong quantity)
        {
            await this.productRepository
                .AddAsync(new()
                {
                    Name = name,
                    ImageUrl = imageUrl,
                    CategoryId = categoryId,
                    Price = price,
                    Description = description,
                    Quantity = quantity
                });

            await this.productRepository
                .SaveChangesAsync();
        }

        public async Task<Tuple<IEnumerable<ProductServiceModel>, int, int, int>> All(
            string searchTerm, 
            int categoryFilterId,
            byte orderNumber,
            int pageNumber,
            int pageSize,
            bool onlyAvailable = true)
        {
            var (searchTermResult,
                categoryFilterIdResult,
                orderNumberResult,
                pageNumberResult) = await 
                ValidateAndSetDefaultSearchParameters(
                    searchTerm, 
                    categoryFilterId, 
                    orderNumber, 
                    pageNumber,
                    pageSize,
                    onlyAvailable,
                    this.productCategoryService,
                    this.productRepository);

            var productsWithCategories = await this.productRepository
                .GetAllProductsWithCategories(
                    searchTermResult,
                    categoryFilterIdResult,
                    orderNumberResult,
                    pageNumberResult,
                    pageSize,
                    onlyAvailable);

            return new(productsWithCategories.ProjectTo<ProductServiceModel>(this.mapper),
                pageNumberResult,
                (int) this.LastPageNumber,
                categoryFilterIdResult);
        }

        public async Task<IEnumerable<ProductServiceModel>> All(string[] ids)
        {
            var products = await this.productRepository.All(ids);

            return products.ProjectTo<ProductServiceModel>(this.mapper);
        }

        public async Task<ProductServiceModel> GetByIdOrDefault(string id)
        {
            if (!await IsProductExist(id))
            {
                return null;
            }

            var product = await this.productRepository
                .GetProductWithCategoryById(id);

            return this.mapper.Map<ProductServiceModel>(product);
        }

        public async Task<bool> IsProductExist(string productId)
            => await this.productRepository
                .FindOrDefaultAsync(p => p.Id == productId) != null;

        public async Task<decimal> PriceForProducts(params string[] ids)
            => (await this.productRepository.All(ids))
                .Sum(p => p.Price);

        public async Task<bool> Update(
            string id, 
            string name, 
            string imageUrl, 
            int categoryId,
            decimal price, 
            string description,
            ulong quantity)
        {
            var product = await this.productRepository
                .GetProductWithCategoryById(id);

            var category = await this.productCategoryService
                .GetByIdOrDefault(categoryId);

            if (product.Category.Title != category.Title
                || product.Name != name)
            {
                if (await this.productCategoryService.IsItemExistInCategory(categoryId, name))
                {
                    return false;
                }
            }

            product.Name = name;
            product.ImageUrl = imageUrl;
            product.CategoryId = categoryId;
            product.Price = price;
            product.Description = description;
            product.Quantity = quantity;

            this.productRepository.Update(product);

            await this.productRepository.SaveChangesAsync();

            return true;
        }
    }
}

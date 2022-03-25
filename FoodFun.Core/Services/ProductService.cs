namespace FoodFun.Core.Services
{
    using Contracts;
    using Extensions;
    using global::AutoMapper;
    using Infrastructure.Common.Contracts;
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

        private double LastPageNumber { get; set; }

        public async Task Add(
            string name, 
            string imageUrl, 
            int categoryId, 
            decimal price, 
            string description,
            long quantity)
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
                    onlyAvailable);

            var productsWithCategories = await this.productRepository
                .GetAllProductsWithCategories(
                    searchTermResult,
                    categoryFilterIdResult,
                    orderNumberResult,
                    pageNumberResult,
                    pageSize,
                    onlyAvailable);

            return new(productsWithCategories
                .ProjectTo<ProductServiceModel>(this.mapper),
                pageNumberResult,
                (int) this.LastPageNumber,
                categoryFilterIdResult);
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

        public async Task<bool> Update(
            string id, 
            string name, 
            string imageUrl, 
            int categoryId,
            decimal price, 
            string description,
            long quantity)
        {
            var product = await this.productRepository
                .GetProductWithCategoryById(id);

            var category = await this.productCategoryService
                .GetByIdOrDefault(categoryId);

            if (product.Category.Title != category.Title
                || product.Name != name)
            {
                if (await this.productCategoryService.IsProductExistInCategory(categoryId, name))
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

        public async Task<bool> IsProductExist(string productId)
            => await this.productRepository
                .FindOrDefaultAsync(p => p.Id == productId) != null;

        private async Task<Tuple<string, int, byte, int>> ValidateAndSetDefaultSearchParameters(
            string searchTerm,
            int categoryFilterId,
            byte orderNumber,
            int pageNumber,
            int pageSize,
            bool onlyAvailable)
        {
            string searchTermResult = null;
            int categoryFilterIdResult = 0;
            byte orderNumberResult = 0;
            int pageNumberResult = 1;

            if (searchTerm != null)
            {
                searchTermResult = searchTerm;
            }

            if (await this.productCategoryService.IsCategoryExist(categoryFilterId))
            {
                categoryFilterIdResult = categoryFilterId;
            }

            if (orderNumber == 1)
            {
                orderNumberResult = orderNumber;
            }

            await PopulateLastPageNumberByFilter(
                searchTermResult, 
                categoryFilterIdResult,
                pageSize,
                onlyAvailable);

            if (pageNumber > 0 
                && pageNumber <= this.LastPageNumber)
            {
                pageNumberResult = pageNumber;
            }

            return new(
                searchTermResult, 
                categoryFilterIdResult, 
                orderNumberResult,
                pageNumberResult);
        }

        private async Task PopulateLastPageNumberByFilter(
            string searchTerm,
            int categoryFilterId,
            int pageSize,
            bool onlyAvailable)
        {
            var numberOfPagesByFilter = await this.productRepository
                .GetNumberOfPagesByFilter(
                    searchTerm, 
                    categoryFilterId,
                    onlyAvailable);

            this.LastPageNumber = Math.Ceiling(numberOfPagesByFilter / (pageSize * 1.0));
        }
    }
}

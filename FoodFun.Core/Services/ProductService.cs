namespace FoodFun.Core.Services
{
    using Contracts;
    using Extensions;
    using global::AutoMapper;
    using Infrastructure.Common;
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

        private double LastPageNumber { get; set; }

        public async Task<Tuple<bool, bool>> AddProduct(
            string name, 
            string imageUrl, 
            int categoryId, 
            decimal price, 
            string description,
            long quantity)
        {
            if (!await this.productCategoryService.IsCategoryExist(categoryId))
            {
                return new(false, false);
            }

            if (!await this.productCategoryService.IsCategoryActive(categoryId))
            {
                return new(false, false);
            }

            if (await IsProductExistInCategory(name, categoryId))
            {
                return new(true, true);
            }

            var product = new Product()
            {
                Name = name,
                ImageUrl = imageUrl,
                CategoryId = categoryId,
                Price = price,
                Description = description,
                Quantity = quantity
            };

            await this.productRepository
                .AddAsync(product);

            await this.productRepository
                .SaveChangesAsync();

            return new(true, false);
        }

        public async Task<Tuple<IEnumerable<ProductServiceModel>, int, int, int>> All(
            string searchTerm, 
            int categoryFilterId,
            byte orderNumber,
            int pageNumber)
        {
            var (searchTermResult,
                categoryFilterIdResult,
                orderNumberResult,
                pageNumberResult) = await 
                ValidateAndSetDefaultSearchParameters(searchTerm, categoryFilterId, orderNumber, pageNumber);

            var productsWithCategories = await this.productRepository
                .GetAllProductsWithCategories(
                    searchTermResult,
                    categoryFilterIdResult,
                    orderNumberResult,
                    pageNumberResult);

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

            if (product.Category.IsDisable)
            {
                return null;
            }

            return this.mapper.Map<ProductServiceModel>(product);
        }

        public async Task<Tuple<bool, bool, bool>> Update(
            string id, 
            string name, 
            string imageUrl, 
            int categoryId,
            decimal price, 
            string description)
        {
            if (!await this.productCategoryService.IsCategoryExist(categoryId))
            {
                return new(false, false, false);
            }

            if (!await this.productCategoryService.IsCategoryActive(categoryId))
            {
                return new(false, false, false);
            }

            if (!await IsProductExist(id))
            {
                return new(true, false, false);
            }

            if (await this.productCategoryService.IsProductInCategory(categoryId, name))
            {
                return new(true, true, true);
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

            return new(true, true, false);
        }

        public async Task<bool> Delete(string id)
        {
            if (!await IsProductExist(id))
            {
                return false;
            }

            this.productRepository
                .Remove(new Product() { Id = id} );

            await this.productRepository
                .SaveChangesAsync();

            return true;
        }

        private async Task<bool> IsProductExist(string productId)
            => await this.productRepository
                .FindOrDefaultAsync(p => p.Id == productId) != null;

        private async Task<Tuple<string, int, byte, int>> ValidateAndSetDefaultSearchParameters(
            string searchTerm,
            int categoryFilterId,
            byte orderNumber,
            int pageNumber)
        {
            string searchTermResult = null;
            int categoryFilterIdResult = 0;
            byte orderNumberResult = 0;
            int pageNumberResult = 1;

            if (searchTerm != null)
            {
                searchTermResult = searchTerm;
            }

            if (await this.productCategoryService.IsCategoryExist(categoryFilterId)
                && await this.productCategoryService.IsCategoryActive(categoryFilterId))
            {
                categoryFilterIdResult = categoryFilterId;
            }

            if (orderNumber == 1)
            {
                orderNumberResult = orderNumber;
            }

            await PopulateLastPageNumberByFilter(searchTermResult, categoryFilterIdResult);

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
            int categoryFilterId)
        {
            var numberOfPagesByFilter = await this.productRepository
                .GetNumberOfPagesByFilter(searchTerm, categoryFilterId);

            this.LastPageNumber = Math.Ceiling(numberOfPagesByFilter / (DataConstants.ItemPerPage * 1.0));
        }

        private async Task<bool> IsProductExistInCategory(
            string productName,
            int categoryId)
            => await this.productRepository
                .FindOrDefaultAsync(x => x.Name == productName 
                                         && x.CategoryId == categoryId) != null;
    }
}

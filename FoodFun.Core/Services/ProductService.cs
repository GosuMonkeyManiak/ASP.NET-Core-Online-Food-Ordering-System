﻿namespace FoodFun.Core.Services
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

        public async Task<Tuple<IEnumerable<ProductServiceModel>, int, int>> All(
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
                (int) this.LastPageNumber);
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

            if (await this.productCategoryService.IsCategoryExist(categoryFilterId))
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
    }
}

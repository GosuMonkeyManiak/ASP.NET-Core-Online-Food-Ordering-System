﻿namespace FoodFun.Core.Services
{
    using Contracts;
    using Extensions;
    using global::AutoMapper;
    using Infrastructure.Common.Contracts;
    using Infrastructure.Models;
    using Models.ProductCategory;

    public class ProductCategoryService : IProductCategoryService
    {
        private readonly IProductCategoryRepository productCategoryRepository;
        private readonly IMapper mapper;

        public ProductCategoryService(
            IProductCategoryRepository productCategoryRepository, 
            IMapper mapper)
        {
            this.productCategoryRepository = productCategoryRepository;
            this.mapper = mapper;
        }

        public async Task<bool> Add(string title)
        {
            var isCategoryExist = this.productCategoryRepository
                .FindOrDefaultAsync(x => x.Title == title) != null;

            if (isCategoryExist)
            {
                return false;
            }

            await this.productCategoryRepository
                .AddAsync(new() { Title = title });

            await this.productCategoryRepository
                .SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<ProductCategoryServiceModel>> All()
        {
            var categoriesForProduct = await this.productCategoryRepository
                .AllAsNoTracking();

            return categoriesForProduct.ProjectTo<ProductCategoryServiceModel>(this.mapper);
        }

        public async Task<IEnumerable<ProductCategoryServiceModel>> AllNotDisabled()
        {
            var notDisableCategories = await this.productCategoryRepository
                .GetAllNotDisabled();

            return notDisableCategories.ProjectTo<ProductCategoryServiceModel>(this.mapper);
        }

        public async Task<bool> IsCategoryActive(int id)
            => !(await this.productCategoryRepository
                    .FindOrDefaultAsync(x => x.Id == id))
                .IsDisable;

        public async Task<IEnumerable<ProductCategoryWithProductCountServiceModel>> AllWithProductsCount()
        {
            var categoriesWithProducts = await this.productCategoryRepository
                .GetAllCategoriesWithProducts();

            return categoriesWithProducts.ProjectTo<ProductCategoryWithProductCountServiceModel>(this.mapper);
        }

        public async Task<bool> IsCategoryExist(int id)
            => await this.productCategoryRepository
                .FindOrDefaultAsync(c => c.Id == id) != null;

        public async Task<ProductCategoryServiceModel> GetByIdOrDefault(int id)
        {
            if (!await IsCategoryExist(id))
            {
                return null;
            }

            var productCategory = await this.productCategoryRepository
                .FindOrDefaultAsync(x => x.Id == id);

            return this.mapper.Map<ProductCategoryServiceModel>(productCategory);
        }

        public async Task<Tuple<bool, bool>> Update(int categoryId, string title)
        {
            if (!await IsCategoryExist(categoryId))
            {
                return new(false, false);
            }

            var isCategoryExistWithThatTitle = await this.productCategoryRepository
                .FindOrDefaultAsync(x => x.Title == title) != null;

            if (isCategoryExistWithThatTitle)
            {
                return new(true, true);
            }

            var productCategory = new ProductCategory()
            {
                Id = categoryId,
                Title = title
            };

            this.productCategoryRepository.Update(productCategory);

            await this.productCategoryRepository.SaveChangesAsync();

            return new(true, false);
        }

        public async Task<bool> IsProductInCategory(int categoryId, string productName)
            => (await this.productCategoryRepository
                    .GetCategoryWithProductsById(categoryId))
                .Products
                .Any(x => x.Name == productName);

        public async Task<bool> Disable(int id)
        {
            if (!await IsCategoryExist(id))
            {
                return false;
            }

            var category = await this.productCategoryRepository
                .FindOrDefaultAsync(x => x.Id == id);

            category.IsDisable = true;

            this.productCategoryRepository
                .Update(category);

            await this.productCategoryRepository
                .SaveChangesAsync();

            return true;
        }

        public async Task<bool> Enable(int id)
        {
            if (!await IsCategoryExist(id))
            {
                return false;
            }

            var category = await this.productCategoryRepository
                .FindOrDefaultAsync(x => x.Id == id);

            category.IsDisable = false;

            this.productCategoryRepository
                .Update(category);

            await this.productCategoryRepository
                .SaveChangesAsync();

            return true;
        }
    }
}

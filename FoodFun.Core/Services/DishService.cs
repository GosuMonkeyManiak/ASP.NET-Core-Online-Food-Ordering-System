﻿namespace FoodFun.Core.Services
{
    using Contracts;
    using Infrastructure.Data;
    using Infrastructure.Models;

    public class DishService : IDishService
    {
        private readonly FoodFunDbContext dbContext;
        private readonly IDishCategoryService dishCategoryService;

        public DishService(
            FoodFunDbContext dbContext, 
            IDishCategoryService dishCategoryService)
        {
            this.dbContext = dbContext;
            this.dishCategoryService = dishCategoryService;
        }

        public async Task<bool> Add(
            string name, 
            string imageUrl, 
            int categoryId, 
            decimal price, 
            string description)
        {
            if (!await this.dishCategoryService.IsCategoryExist(categoryId))
            {
                return false;
            }

            var dish = new Dish()
            {
                Name = name,
                ImageUrl = imageUrl,
                CategoryId = categoryId,
                Price = price,
                Description = description
            };

            await this.dbContext
                .Dishes
                .AddAsync(dish);

            await this.dbContext.SaveChangesAsync();

            return true;
        }
    }
}

﻿namespace FoodFun.Web.Controllers
{
    using Core.Contracts;
    using Core.Extensions;
    using global::AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models.Dish;
    using Models.DishCategory;

    using static Constants.GlobalConstants.Roles;
    using static Constants.GlobalConstants.Redirect;
    using static Constants.GlobalConstants.Messages;

    public class DishController : Controller
    {
        private const string CategoryId = nameof(CategoryId);

        private readonly IDishService dishService;
        private readonly IDishCategoryService dishCategoryService;
        private readonly IMapper mapper;

        public DishController(
            IDishService dishService, 
            IDishCategoryService dishCategoryService,
            IMapper mapper)
        {
            this.dishService = dishService;
            this.dishCategoryService = dishCategoryService;
            this.mapper = mapper;
        }

        [Authorize(Roles = Administrator)]
        public async Task<IActionResult> Add()
            => View(new DishFormModel() { Categories = await GetDishCategories() });

        [HttpPost]
        [Authorize(Roles = Administrator)]
        public async Task<IActionResult> Add(DishFormModel formModel)
        {
            if (!this.ModelState.IsValid)
            {
                formModel.Categories = await GetDishCategories();

                return View(formModel);
            }

            var isSucceed = await this.dishService
                .Add(
                    formModel.Name,
                    formModel.ImageUrl,
                    formModel.CategoryId,
                    formModel.Price,
                    formModel.Description);

            if (!isSucceed)
            {
                this.ModelState.AddModelError(CategoryId, DishCategoryNotExist);

                formModel.Categories = await GetDishCategories();

                return View(formModel);
            }

            return Redirect(HomeIndexUrl);
        }

        [Authorize(Roles = $"{Administrator}, {Customer}")]
        public IActionResult All()
        {
            return null;
        }

        private async Task<IEnumerable<DishCategoryModel>> GetDishCategories()
        {
            var dishCategoriesFromService = await this.dishCategoryService.All();

            return dishCategoriesFromService.ProjectTo<DishCategoryModel>(this.mapper);
        }
    }
}

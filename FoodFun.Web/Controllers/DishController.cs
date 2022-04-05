namespace FoodFun.Web.Controllers
{
    using FoodFun.Core.Contracts;
    using FoodFun.Core.Extensions;
    using FoodFun.Infrastructure.Common;
    using FoodFun.Web.Areas.Restaurant.Models.Dish;
    using FoodFun.Web.Areas.Restaurant.Models.DishCategory;
    using FoodFun.Web.Models.Dish;
    using global::AutoMapper;
    using Microsoft.AspNetCore.Mvc;

    public class DishController : Controller
    {
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

        public async Task<IActionResult> All(DishSearchModel searchModel)
        {
            var (filteredDishes,
                currentPageNumber,
                lastPageNumber,
                selectedCategoryId) = await this.dishService
                .All(
                    searchModel.SearchTerm,
                    searchModel.CategoryId,
                    searchModel.OrderNumber,
                    searchModel.CurrentPageNumber,
                    DataConstants.PublicPageSize);

            var model = new DishSearchModel()
            {
                CurrentPageNumber = currentPageNumber,
                LastPageNumber = lastPageNumber,
                SelectedCategoryId = selectedCategoryId,
                Dishes = filteredDishes.ProjectTo<DishListingModel>(this.mapper),
                Categories = (await this.dishCategoryService.AllNotDisabled()).ProjectTo<DishCategoryModel>(this.mapper)
            };

            return View(model);
        }
    }
}

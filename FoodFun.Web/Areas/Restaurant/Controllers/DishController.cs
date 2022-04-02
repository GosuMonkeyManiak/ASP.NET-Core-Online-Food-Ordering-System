namespace FoodFun.Web.Areas.Restaurant.Controllers
{
    using Core.Contracts;
    using Core.Extensions;
    using global::AutoMapper;
    using Infrastructure.Common;
    using Microsoft.AspNetCore.Mvc;
    using Models.Dish;
    using Models.DishCategory;
    using static Constants.GlobalConstants.Messages;

    public class DishController : RestaurantBaseController
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
        
        public async Task<IActionResult> Add()
            => View(new DishFormModel()
            {
                Categories = await GetDishCategories()
            });

        [HttpPost]
        public async Task<IActionResult> Add(DishFormModel formModel)
        {
            if (!this.ModelState.IsValid)
            {
                formModel.Categories = await GetDishCategories();

                return View(formModel);
            }

            await this.dishService
                .Add(
                    formModel.Name,
                    formModel.ImageUrl,
                    formModel.CategoryId,
                    formModel.Price,
                    formModel.Description,
                    formModel.Quantity);

            return RedirectToAction(nameof(All));
        }
        
        public async Task<IActionResult> All([FromQuery] DishSearchModel searchModel)
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
                    DataConstants.PrivatePageSize,
                    onlyAvailable: false);

            var categoriesForProduct = await GetDishCategories();

            var productSearchModel = new DishSearchModel()
            {
                CurrentPageNumber = currentPageNumber,
                LastPageNumber = lastPageNumber,
                SelectedCategoryId = selectedCategoryId,
                Dishes = filteredDishes.ProjectTo<DishListingModel>(this.mapper),
                Categories = categoriesForProduct
            };

            return View(productSearchModel);
        }
        
        public async Task<IActionResult> Edit(string id)
        {
            var dishWithCategory = await this.dishService
                .GetByIdOrDefault(id);

            if (dishWithCategory == null)
            {
                this.TempData[Error] = DishNotExit;

                return RedirectToAction(nameof(All));
            }

            var dishEditModel = this.mapper.Map<DishEditModel>(dishWithCategory);
            dishEditModel.Categories = await GetDishCategories();

            return View(dishEditModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(DishEditModel editModel)
        {
            if (!this.ModelState.IsValid)
            {
                editModel.Categories = await GetDishCategories();

                return View(editModel);
            }

            var isSucceed = await this.dishService
                .Update(
                    editModel.Id,
                    editModel.Name,
                    editModel.ImageUrl,
                    editModel.CategoryId,
                    editModel.Price,
                    editModel.Description,
                    editModel.Quantity);

            if (!isSucceed)
            {
                this.TempData[Error] = DishAlreadyExistInCategory;
            }

            return RedirectToAction(nameof(All));
        }

        private async Task<IEnumerable<DishCategoryModel>> GetDishCategories()
        {
            var dishCategoriesFromService = await this.dishCategoryService.All();

            return dishCategoriesFromService.ProjectTo<DishCategoryModel>(this.mapper);
        }
    }
}

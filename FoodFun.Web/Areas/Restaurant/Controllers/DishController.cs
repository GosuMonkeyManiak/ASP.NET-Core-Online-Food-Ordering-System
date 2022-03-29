namespace FoodFun.Web.Areas.Restaurant.Controllers
{
    using Core.Contracts;
    using Core.Extensions;
    using global::AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using Models.Dish;
    using Models.DishCategory;

    using static Constants.GlobalConstants;
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

            var (isCategoryExist,
                isDishInCategory) = await this.dishService
                .Add(
                    formModel.Name,
                    formModel.ImageUrl,
                    formModel.CategoryId,
                    formModel.Price,
                    formModel.Description);

            return RedirectToAction(nameof(All));
        }
        
        public async Task<IActionResult> All()
        {
            var dishesWithCategoriesFromService = await this.dishService
                .All();

            return View(dishesWithCategoriesFromService.ProjectTo<DishListingModel>(this.mapper));
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
                    editModel.Description);

            if (!isSucceed)
            {
                this.TempData[Error] = DishAndCategoryNotExit;
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

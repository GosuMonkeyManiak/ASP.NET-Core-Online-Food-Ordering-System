namespace FoodFun.Web.Areas.Restaurant.Controllers
{
    using Core.Contracts;
    using Core.Extensions;
    using global::AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using Models.DishCategory;

    using static Constants.GlobalConstants.Messages;

    public class DishCategoryController : RestaurantBaseController
    {
        private readonly IDishCategoryService dishCategoryService;
        private readonly IMapper mapper;

        public DishCategoryController(
            IDishCategoryService dishCategoryService, 
            IMapper mapper)
        {
            this.dishCategoryService = dishCategoryService;
            this.mapper = mapper;
        }

        public IActionResult Add()
            => View();

        [HttpPost]
        public async Task<IActionResult> Add(DishCategoryFormModel formModel)
        {
            if (!this.ModelState.IsValid)
            {
                return View();
            }

            var isSucceed = await this.dishCategoryService
                .Add(formModel.Title);

            if (!isSucceed)
            {
                this.TempData[Error] = DishCategoryAlreadyExist;

                return View();
            }

            return RedirectToAction(nameof(All));
        }

        public async Task<IActionResult> All()
        {
            var categoriesWithDishCount = await this.dishCategoryService
                .AllWithDishesCount();

            return View(categoriesWithDishCount.ProjectTo<DishCategoryListingModel>(this.mapper));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var dishCategory = await this.dishCategoryService
                .GetByIdOrDefault(id);

            if (dishCategory == null)
            {
                this.TempData[Error] = DishCategoryNotExist;

                return RedirectToAction(nameof(All));
            }

            return View(this.mapper.Map<DishCategoryEditModel>(dishCategory));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(DishCategoryEditModel editModel)
        {
            if (!this.ModelState.IsValid)
            {
                return View(editModel);
            }

            var isSucceed = await this.dishCategoryService
                .Update(
                    editModel.Id,
                    editModel.Title);

            if (!isSucceed)
            {
                this.TempData[Error] = DishCategoryNotExist;
            }

            return RedirectToAction(nameof(All));
        }
    }
}

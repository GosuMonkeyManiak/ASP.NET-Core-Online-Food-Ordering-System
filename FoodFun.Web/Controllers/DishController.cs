namespace FoodFun.Web.Controllers
{
    using Core.Contracts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models.Dish;
    using Models.DishCategory;

    using static Constants.GlobalConstants.Roles;

    public class DishController : Controller
    {
        private readonly IDishService dishService;
        private readonly IDishCategoryService dishCategoryService;

        public DishController(
            IDishService dishService, 
            IDishCategoryService dishCategoryService)
        {
            this.dishService = dishService;
            this.dishCategoryService = dishCategoryService;
        }

        [Authorize(Roles = Administrator)]
        public async Task<IActionResult> Add()
        {
            var dishCategoriesFromService = await this.dishCategoryService.All();

            var dishCategories = dishCategoriesFromService
                .Select(dc => new DishCategoryModel()
                {
                    Id = dc.Id,
                    Title = dc.Title
                })
                .ToList();

            return View(new DishFormModel()
            {
                Categories = dishCategories
            });
        }

        [HttpPost]
        [Authorize(Roles = Administrator)]
        public async Task<IActionResult> Add(DishFormModel formModel)
        {
            if (!ModelState.IsValid)
            {
                var dishCategoriesFromService = await this.dishCategoryService.All();

                var dishCategories = dishCategoriesFromService
                    .Select(dc => new DishCategoryModel()
                    {
                        Id = dc.Id,
                        Title = dc.Title
                    })
                    .ToList();

                formModel.Categories = dishCategories;

                return View(formModel);
            }

            var (isSucceed, errors) = await this.dishService
                .Add(
                    formModel.Name,
                    formModel.ImageUrl,
                    formModel.CategoryId,
                    formModel.Price,
                    formModel.Description);

            if (!isSucceed)
            {
                foreach (var error in errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }

                var dishCategoriesFromService = await this.dishCategoryService.All();

                var dishCategories = dishCategoriesFromService
                    .Select(dc => new DishCategoryModel()
                    {
                        Id = dc.Id,
                        Title = dc.Title
                    })
                    .ToList();

                formModel.Categories = dishCategories;

                return View(formModel);
            }

            return Redirect("/Home/Index");
        }
    }
}

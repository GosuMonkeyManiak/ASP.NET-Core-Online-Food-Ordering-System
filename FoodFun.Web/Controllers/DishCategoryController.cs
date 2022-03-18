namespace FoodFun.Web.Controllers
{
    using Core.Contracts;
    using Core.Extensions;
    using global::AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models.DishCategory;
    
    using static Constants.GlobalConstants.Roles;
    using static Constants.GlobalConstants.Messages;

    [Authorize(Roles = Administrator)]
    public class DishCategoryController : Controller
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
                this.TempData[nameof(Error)] = DishCategoryAlreadyExist;
            }

            return RedirectToAction(nameof(All));
        }

        public async Task<IActionResult> All()
        {
            var categoriesWithDishCount = await this.dishCategoryService
                .AllWithDishesCount();

            return View(categoriesWithDishCount.ProjectTo<DishCategoryListingModel>(this.mapper));
        }
    }
}

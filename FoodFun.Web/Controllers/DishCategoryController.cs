namespace FoodFun.Web.Controllers
{
    using Core.Contracts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models.DishCategory;
    
    using static Constants.GlobalConstants.Roles;
    using static Constants.GlobalConstants.Messages;

    [Authorize(Roles = Administrator)]
    public class DishCategoryController : Controller
    {
        private readonly IDishCategoryService dishCategoryService;

        public DishCategoryController(IDishCategoryService dishCategoryService) 
            => this.dishCategoryService = dishCategoryService;

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

            return RedirectToAction(); //All
        }
    }
}

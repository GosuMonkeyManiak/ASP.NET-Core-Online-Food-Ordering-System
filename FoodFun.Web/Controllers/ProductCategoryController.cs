namespace FoodFun.Web.Controllers
{
    using Core.Contracts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models.ProductCategory;
    using static Constants.GlobalConstants.Roles;

    [Authorize(Roles = Administrator)]
    public class ProductCategoryController : Controller
    {
        private readonly IProductCategoryService productCategoryService;

        public ProductCategoryController(IProductCategoryService productCategoryService)
            => this.productCategoryService = productCategoryService;

        public async Task<IActionResult> Add()
            => View();

        [HttpPost]
        public async Task<IActionResult> Add(ProductCategoryFormModel formModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            await this.productCategoryService.Add(formModel.Title);

            return RedirectToAction(nameof(Add));
        }

        public async Task<IActionResult> All()
        {
            var productCategoriesFromService = await this.productCategoryService.All();

            var productCategories = productCategoriesFromService
                .Select(p => new ProductCategoryModel()
                {
                    Id = p.Id,
                    Title = p.Title
                })
                .ToList();

            return View(productCategories);
        }
    }
}

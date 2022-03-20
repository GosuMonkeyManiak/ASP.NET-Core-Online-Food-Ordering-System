namespace FoodFun.Web.Controllers
{
    using Core.Contracts;
    using Core.Extensions;
    using global::AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models.ProductCategory;
    using static Constants.GlobalConstants.Roles;
    using static Constants.GlobalConstants.Messages;

    [Authorize(Roles = Administrator)]
    public class ProductCategoryController : Controller
    {
        private readonly IProductCategoryService productCategoryService;
        private readonly IMapper mapper;

        public ProductCategoryController(
            IProductCategoryService productCategoryService, 
            IMapper mapper)
        {
            this.productCategoryService = productCategoryService;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Add()
            => View();

        [HttpPost]
        public async Task<IActionResult> Add(ProductCategoryFormModel formModel)
        {
            if (!this.ModelState.IsValid)
            {
                return View();
            }

            var isSucceed = await this.productCategoryService.Add(formModel.Title);

            if (!isSucceed)
            {
                this.TempData[Error] = ProductCategoryAlreadyExist;

                return View();
            }

            return RedirectToAction(nameof(All));
        }

        public async Task<IActionResult> All()
        {
            var categoriesWithProductsCount = await this.productCategoryService
                .AllWithProductsCount();

            return View(categoriesWithProductsCount
                .ProjectTo<ProductCategoryListingModel>(this.mapper));
        }

        public async Task<IActionResult> Edit(int productCategoryId)
        {
            var productCategoryServiceModel = await this.productCategoryService
                .GetByIdOrDefault(productCategoryId);

            if (productCategoryServiceModel == null)
            {
                TempData[Error] = ProductCategoryNotExist;

                return RedirectToAction(nameof(All));
            }

            return View(this.mapper.Map<ProductCategoryEditModel>(productCategoryServiceModel));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductCategoryEditModel productCategoryModel)
        {
            if (!this.ModelState.IsValid)
            {
                return View(productCategoryModel);
            }

            var isSucceed = await this.productCategoryService
                .Update(
                    productCategoryModel.Id,
                    productCategoryModel.Title);

            if (!isSucceed)
            {
                TempData[Error] = ProductCategoryNotExist;
            }

            return RedirectToAction(nameof(All));
        }
    }
}

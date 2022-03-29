namespace FoodFun.Web.Areas.Supermarket.Controllers
{
    using Core.Contracts;
    using Core.Extensions;
    using Core.ValidationAttributes.ProductCategory;
    using global::AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using Models.ProductCategory;

    using static Constants.GlobalConstants.Messages;

    public class ProductCategoryController : SupermarketBaseController
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

        public IActionResult Add()
            => View();

        [HttpPost]
        public async Task<IActionResult> Add(ProductCategoryFormModel formModel)
        {
            if (!this.ModelState.IsValid)
            {
                return View();
            }

            await this.productCategoryService.Add(formModel.Title);

            return RedirectToAction(nameof(All));
        }

        public async Task<IActionResult> All()
        {
            var categoriesWithProductsCount = await this.productCategoryService
                .AllWithProductsCount();

            return View(categoriesWithProductsCount
                .ProjectTo<ProductCategoryListingModel>(this.mapper));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var productCategoryServiceModel = await this.productCategoryService
                .GetByIdOrDefault(id);

            if (productCategoryServiceModel == null)
            {
                this.TempData[Error] = ProductCategoryNotExist;

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

            await this.productCategoryService
                .Update(
                    productCategoryModel.Id,
                    productCategoryModel.Title);

            return RedirectToAction(nameof(All));
        }

        public async Task<IActionResult> Disable([MustBeExistingProductCategory] int id)
        {
            if (!this.ModelState.IsValid)
            {
                this.TempData[Error] = this.ModelState.Values
                    .First()
                    .Errors
                    .First().ErrorMessage;

                return RedirectToAction(nameof(All));
            }

            await this.productCategoryService.Disable(id);

            return RedirectToAction(nameof(All));
        }

        public async Task<IActionResult> Enable([MustBeExistingProductCategory] int id)
        {
            if (!this.ModelState.IsValid)
            {
                this.TempData[Error] = this.ModelState.Values
                    .First()
                    .Errors
                    .First().ErrorMessage;

                return RedirectToAction(nameof(All));
            }

            await this.productCategoryService.Enable(id);

            return RedirectToAction(nameof(All));
        }
    }
}

namespace FoodFun.Web.Areas.Supermarket.Controllers
{
    using Core.Contracts;
    using Core.Extensions;
    using global::AutoMapper;
    using Infrastructure.Common;
    using Microsoft.AspNetCore.Mvc;
    using Models.Product;
    using Web.Models.Product;
    
    using static Constants.GlobalConstants.Messages;

    public class ProductController : SupermarketBaseController
    {
        private readonly IProductService productService;
        private readonly IProductCategoryService productCategoryService;
        private readonly IMapper mapper;

        public ProductController(
            IProductService productService,
            IProductCategoryService productCategoryService,
            IMapper mapper)
        {
            this.productService = productService;
            this.productCategoryService = productCategoryService;
            this.mapper = mapper;
        }
        
        public async Task<IActionResult> Add()
            => View(new ProductFormModel()
            {
                Categories = await GetProductCategories()
            });

        [HttpPost]
        public async Task<IActionResult> Add(ProductFormModel formModel)
        {
            if (!this.ModelState.IsValid)
            {
                formModel.Categories = await GetProductCategories();

                return View(formModel);
            }

            await this.productService
                .Add(
                    formModel.Name,
                    formModel.ImageUrl,
                    formModel.CategoryId,
                    formModel.Price,
                    formModel.Description,
                    formModel.Quantity);

            return RedirectToAction(nameof(All));
        }

        public async Task<IActionResult> All([FromQuery] ProductSearchModel searchModel)
        {
            var (filteredProducts,
                currentPageNumber,
                lastPageNumber,
                selectedCategoryId) = await this.productService
                .All(
                    searchModel.SearchTerm,
                    searchModel.CategoryId,
                    searchModel.OrderNumber,
                    searchModel.CurrentPageNumber,
                    DataConstants.SupermarketPageSize,
                    onlyAvailable: false);

            var categoriesForProduct = await GetProductCategories();

            var productSearchModel = new ProductSearchModel()
            {
                CurrentPageNumber = currentPageNumber,
                LastPageNumber = lastPageNumber,
                SelectedCategoryId = selectedCategoryId,
                Products = filteredProducts.ProjectTo<ProductListingModel>(this.mapper),
                Categories = categoriesForProduct
            };

            return View(productSearchModel);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var productServiceModel = await this.productService
                .GetByIdOrDefault(id);

            if (productServiceModel == null)
            {
                this.TempData[Error] = ProductNotExist;

                return RedirectToAction(nameof(All));
            }

            var productEditModel = this.mapper.Map<ProductEditModel>(productServiceModel);
            productEditModel.Categories = await GetProductCategories();

            return View(productEditModel);
        }
        
        [HttpPost]
        public async Task<IActionResult> Edit(ProductEditModel editModel)
        {
            if (!this.ModelState.IsValid)
            {
                editModel.Categories = await GetProductCategories();

                return View(editModel);
            }

            var isSucceed = await this.productService
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
                this.TempData[Error] = ProductAlreadyExistInCategory;
            }

            return RedirectToAction(nameof(All));
        }

        private async Task<IEnumerable<ProductCategoryModel>> GetProductCategories()
        {
            var categoriesForProduct = await this.productCategoryService.All();

            return categoriesForProduct.ProjectTo<ProductCategoryModel>(this.mapper);
        }
    }
}

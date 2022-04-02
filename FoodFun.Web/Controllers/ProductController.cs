namespace FoodFun.Web.Controllers
{
    using Core.Contracts;
    using Core.Extensions;
    using global::AutoMapper;
    using Infrastructure.Common;
    using Microsoft.AspNetCore.Mvc;
    using Models.Product;

    public class ProductController : Controller
    {
        private readonly IProductService productService;
        private readonly IMapper mapper;
        private readonly IProductCategoryService productCategoryService;

        public ProductController(
            IProductService productService, 
            IMapper mapper, 
            IProductCategoryService productCategoryService)
        {
            this.productService = productService;
            this.mapper = mapper;
            this.productCategoryService = productCategoryService;
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
                    DataConstants.PublicPageSize);

            var categoriesForProduct = await productCategoryService.AllNotDisabled();

            var productSearchModel = new ProductSearchModel()
            {
                CurrentPageNumber = currentPageNumber,
                LastPageNumber = lastPageNumber,
                SelectedCategoryId = selectedCategoryId,
                Products = filteredProducts.ProjectTo<ProductListingModel>(this.mapper),
                Categories = categoriesForProduct.ProjectTo<ProductCategoryModel>(this.mapper)
            };

            return View(productSearchModel);
        }
    }
}

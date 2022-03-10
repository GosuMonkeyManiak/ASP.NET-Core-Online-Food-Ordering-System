namespace FoodFun.Web.Controllers
{
    using Constants;
    using Core.Contracts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models.Product;
    using static Constants.GlobalConstants.Roles;
    
    public class ProductController : Controller
    {
        private readonly IProductService productService;

        public ProductController(IProductService productService)
            => this.productService = productService;

        [Authorize(Roles = Administrator)]
        public async Task<IActionResult> Add() 
            => View(new ProductFormModel()
            {
                Categories = await this.productService.GetCategories()
            });

        [Authorize(Roles = Administrator)]
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Add(ProductFormModel formModel)
        {
            if (!this.ModelState.IsValid)
            {
                return Add().GetAwaiter().GetResult();
            }
             
            var (isSucceed, errors) = await this.productService
                .AddProduct(
                    formModel.Name,
                    formModel.ImageUrl,
                    formModel.CategoryId,
                    formModel.Price,
                    formModel.Description);

            if (!isSucceed)
            {
                foreach (var error in errors)
                {
                    this.ModelState.AddModelError(string.Empty, error);
                }

                return Add().GetAwaiter().GetResult();
            }

            return Redirect(GlobalConstants.Redirect.HomeIndexUrl);
        }

        [Authorize(Roles = $"{Administrator}, {Customer}")]
        public async Task<IActionResult> All()
            => View(await this.productService.All());
    }
}

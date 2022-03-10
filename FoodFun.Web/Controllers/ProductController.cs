namespace FoodFun.Web.Controllers
{
    using Constants;
    using Core.Contracts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models.Product;

    using static Constants.GlobalConstants.Roles;
    using static Constants.GlobalConstants.Messages;
    
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

            return RedirectToAction(nameof(All));
        }

        [Authorize(Roles = $"{Administrator}, {Customer}")]
        public async Task<IActionResult> All()
            => View(await this.productService.All());

        [Authorize(Roles = Administrator)]
        public async Task<IActionResult> Edit(string productId)
        {
            var (isSucceed, productServiceModel) = await this.productService
                .GetById(productId);

            if (!isSucceed)
            {
                this.TempData[nameof(ProductNotExist)] = ProductNotExist;

                return RedirectToAction(nameof(All));
            }

            var productEditModel = new ProductEditModel()
            {
                Id = productServiceModel.Id,
                Name = productServiceModel.Name,
                ImageUrl = productServiceModel.ImageUrl,
                Price = productServiceModel.Price,
                CategoryId = productServiceModel.Category.Id,
                Description = productServiceModel.Description,
                Categories = await this.productService.GetCategories()
            };

            return View(productEditModel);
        }

        [Authorize(Roles = Administrator)]
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Edit(ProductEditModel editModel)
        {
            if (!ModelState.IsValid)
            {
                return View(editModel);
            }

            var isSucceed = await this.productService
                .Update(
                    editModel.Id,
                    editModel.Name,
                    editModel.ImageUrl,
                    editModel.CategoryId,
                    editModel.Price,
                    editModel.Description);

            if (!isSucceed)
            {
                this.TempData[nameof(ProductNotExist)] = ProductNotExist;
                this.TempData[nameof(ProductCategoryNotExist)] = ProductCategoryNotExist;

                return RedirectToAction(nameof(All));
            }

            return RedirectToAction(nameof(All));
        }
    }
}

namespace FoodFun.Web.Controllers
{
    using FoodFun.Core.Contracts;
    using FoodFun.Core.Models.Product;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Distributed;
    using FoodFun.Core.Extensions;

    public class HomeController : Controller
    {
        private const string LatestProductsKey = "LatestProducts";

        private readonly IProductService productService;
        private readonly IDistributedCache cache; 

        public HomeController(
            IProductService productService,
            IDistributedCache cache)
        {
            this.productService = productService;
            this.cache = cache;
        }

        public async Task<IActionResult> Index()
        {
            var latesProducts = this.cache.Get<List<LatestProductServiceModel>>(LatestProductsKey);

            if (latesProducts == null)
            {
                latesProducts = (await this.productService.Latest()).ToList();

                var cacheOptions = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(15));

                this.cache.Set<List<LatestProductServiceModel>>(LatestProductsKey, latesProducts, cacheOptions);
            }

            return View(latesProducts);
        }

        [ResponseCache(
            Duration = 0,
            Location = ResponseCacheLocation.None,
            NoStore = true)]
        public IActionResult Error() 
            => View();
    }
}
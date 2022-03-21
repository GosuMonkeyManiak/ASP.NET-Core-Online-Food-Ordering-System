namespace FoodFun.Web.Areas.Supermarket.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    
    public class HomeController : SupermarketBaseController
    {
        public IActionResult Index() 
            => View();
    }
}

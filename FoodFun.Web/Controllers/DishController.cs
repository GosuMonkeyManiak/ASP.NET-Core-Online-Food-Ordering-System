namespace FoodFun.Web.Controllers
{
    using FoodFun.Core.Contracts;
    using global::AutoMapper;
    using Microsoft.AspNetCore.Mvc;

    public class DishController : Controller
    {
        private readonly IDishService dishService;
        private readonly IDishCategoryService dishCategoryService;
        private readonly IMapper mapper;

        public DishController(
            IDishService dishService,
            IDishCategoryService dishCategoryService,
            IMapper mapper)
        {
            this.dishService = dishService;
            this.dishCategoryService = dishCategoryService;
            this.mapper = mapper;
        }
    }
}

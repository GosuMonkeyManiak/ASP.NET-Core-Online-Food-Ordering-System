namespace FoodFun.Web.Areas.Restaurant.Controllers
{
    using FoodFun.Core.Contracts;
    using FoodFun.Web.Areas.Restaurant.Models.Table;
    using Microsoft.AspNetCore.Mvc;

    public class TablePositionController : Controller
    {
        private readonly ITablePositionService tablePositionService;

        public TablePositionController(ITablePositionService tablePositionService) 
            => this.tablePositionService = tablePositionService;

        public IActionResult Add()
            => View();

        [HttpPost]
        public async Task<IActionResult> Add(TablePositionFormModel tablePosition)
        {
            if (!this.ModelState.IsValid)
            {
                return View();
            }

            await this.tablePositionService.Add(tablePosition.Position);

            return Ok();
        }
    }
}

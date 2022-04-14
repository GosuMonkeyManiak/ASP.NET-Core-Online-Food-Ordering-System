namespace FoodFun.Web.Areas.Restaurant.Controllers
{
    using FoodFun.Core.Contracts;
    using FoodFun.Core.Extensions;
    using FoodFun.Web.Areas.Restaurant.Models.Table;
    using FoodFun.Web.Areas.Restaurant.Models.TablePosition;
    using FoodFun.Web.Areas.Restaurant.Models.TableSize;
    using global::AutoMapper;
    using Microsoft.AspNetCore.Mvc;

    public class TableController : RestaurantBaseController
    {
        private readonly ITableService tableService;
        private readonly ITablePositionService tablePositionService;
        private readonly ITableSizeService tableSizeService;
        private readonly IMapper mapper;

        public TableController(
            ITableService tableService,
            ITablePositionService tablePositionService, 
            ITableSizeService tableSizeService, 
            IMapper mapper)
        {
            this.tableService = tableService;
            this.tablePositionService = tablePositionService;
            this.tableSizeService = tableSizeService;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Add()
            => View(new TableFormModel()
            {
                Sizes = await GetTableSizes(),
                Positions = await GetTablePositions(),
            });

        [HttpPost]
        public async Task<IActionResult> Add(TableFormModel tableformModel)
        {
            if (!this.ModelState.IsValid)
            {
                tableformModel.Sizes = await GetTableSizes();
                tableformModel.Positions = await GetTablePositions();

                return View(tableformModel);
            }

            await this.tableService.Add(tableformModel.SizeId, tableformModel.PositionId);

            return RedirectToAction(nameof(All));
        }

        public async Task<IActionResult> All([FromQuery] TableSearchModel searchModel) 
        {
            searchModel.Tables = (await this.tableService
                .All(searchModel.SearchTerm))
                .ProjectTo<TableListingModel>(this.mapper);

            return View(searchModel);
        }

        private async Task<IEnumerable<TableSizeListingModel>> GetTableSizes()
            => (await this.tableSizeService.All()).ProjectTo<TableSizeListingModel>(this.mapper);

        private async Task<IEnumerable<TablePositionListingModel>> GetTablePositions()
            => (await this.tablePositionService.All()).ProjectTo<TablePositionListingModel>(this.mapper);
    }
}

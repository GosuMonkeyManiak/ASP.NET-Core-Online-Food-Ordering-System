namespace FoodFun.Web.Areas.Restaurant.Controllers
{
    using FoodFun.Core.Contracts;
    using FoodFun.Core.Extensions;
    using FoodFun.Web.Areas.Restaurant.Models.Table;
    using FoodFun.Web.Areas.Restaurant.Models.TablePosition;
    using FoodFun.Web.Areas.Restaurant.Models.TableSize;
    using global::AutoMapper;
    using Microsoft.AspNetCore.Mvc;

    using static Constants.GlobalConstants.Messages;

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

        public async Task<IActionResult> Edit(string id)
        {
            var table = await this.tableService.GetByIdOrDefault(id);

            if (table == null)
            {
                this.TempData[Error] = TableNotExist;

                return RedirectToAction(nameof(All));
            }

            return View(new TableEditModel()
            {
                Id = id,
                Position = table.TablePosition,
                Seats = table.TableSize,
                AllPositions = await GetTablePositions(),
                AllSeats = await GetTableSizes()
            });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TableEditModel editModel)
        {
            if (!this.ModelState.IsValid)
            {
                editModel.AllPositions = await GetTablePositions();
                editModel.AllSeats = await GetTableSizes();

                return View(editModel);
            }

            await this.tableService
                    .Update(editModel.Id, editModel.PositionId, editModel.SizeId);

            return RedirectToAction(nameof(All));
        }

        private async Task<IEnumerable<TableSizeListingModel>> GetTableSizes()
            => (await this.tableSizeService.All()).ProjectTo<TableSizeListingModel>(this.mapper);

        private async Task<IEnumerable<TablePositionListingModel>> GetTablePositions()
            => (await this.tablePositionService.All()).ProjectTo<TablePositionListingModel>(this.mapper);
    }
}

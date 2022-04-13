namespace FoodFun.Web.Areas.Restaurant.Controllers
{
    using FoodFun.Core.Contracts;
    using FoodFun.Core.Extensions;
    using FoodFun.Web.Areas.Restaurant.Models.TableSize;
    using global::AutoMapper;
    using Microsoft.AspNetCore.Mvc;

    using static FoodFun.Web.Constants.GlobalConstants.Messages;

    public class TableSizeController : RestaurantBaseController
    {
        private readonly ITableSizeService tableSizeService;
        private readonly IMapper mapper;

        public TableSizeController(
            ITableSizeService tableSizeService,
            IMapper mapper)
        {
            this.tableSizeService = tableSizeService;
            this.mapper = mapper;
        }

        public IActionResult Add()
            => View();

        [HttpPost]
        public async Task<IActionResult> Add(TableSizeFormModel tableSize)
        {
            if (!this.ModelState.IsValid)
            {
                return View();
            }

            await this.tableSizeService.Add(tableSize.Seats);

            return RedirectToAction(nameof(All));
        }

        public async Task<IActionResult> All()
        {
            var tableSizes = await this.tableSizeService.All();

            return View(tableSizes.ProjectTo<TableSizeListingModel>(this.mapper));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var tableSize = await this.tableSizeService.GetByIdOrDefault(id);

            if (tableSize == null)
            {
                this.TempData[Error] = TableSizeNotExist;

                return RedirectToAction(nameof(All));
            }

            return View(this.mapper.Map<TableSizeEditModel>(tableSize));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TableSizeEditModel editModel)
        {
            if (!this.ModelState.IsValid)
            {
                return View(editModel);
            }

            await this.tableSizeService
                    .Update(editModel.Id, editModel.Seats);

            return RedirectToAction(nameof(All));
        }
    }
}

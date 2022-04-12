namespace FoodFun.Web.Areas.Restaurant.Controllers
{
    using FoodFun.Core.Contracts;
    using FoodFun.Core.Extensions;
    using FoodFun.Web.Areas.Restaurant.Models.TablePosition;
    using global::AutoMapper;
    using Microsoft.AspNetCore.Mvc;

    using static Constants.GlobalConstants.Messages;

    public class TablePositionController : RestaurantBaseController
    {
        private readonly ITablePositionService tablePositionService;
        private readonly IMapper mapper;

        public TablePositionController(
            ITablePositionService tablePositionService,
            IMapper mapper)
        {
            this.tablePositionService = tablePositionService;
            this.mapper = mapper;
        }

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

            return RedirectToAction(nameof(All));
        }

        public async Task<IActionResult> All()
        {
            var tablePostions = await this.tablePositionService.All();

            return View(tablePostions.ProjectTo<TablePositionListingModel>(this.mapper));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var position = await this.tablePositionService.GetByIdOrDefault(id);

            if (position == null)
            {
                this.TempData[Error] = TablePositionNotExist;

                return RedirectToAction(nameof(All));
            }

            return View(this.mapper.Map<TablePositionEditModel>(position));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TablePositionEditModel tablePosition)
        {
            if (!this.ModelState.IsValid)
            {
                return View(tablePosition);
            }

            await this.tablePositionService
                .Update(tablePosition.Id, tablePosition.Position);

            return RedirectToAction(nameof(All));
        }
    }
}

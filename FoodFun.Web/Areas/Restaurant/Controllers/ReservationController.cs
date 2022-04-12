namespace FoodFun.Web.Areas.Restaurant.Controllers
{
    using FoodFun.Core.Contracts;
    using FoodFun.Core.Models.Table;
    using Microsoft.AspNetCore.Mvc;

    using static Constants.GlobalConstants.Messages;

    public class ReservationController : RestaurantBaseController
    {
        private readonly IReservationService reservationService;

        public ReservationController(IReservationService reservationService) 
            => this.reservationService = reservationService;

        public IActionResult Index()
            => View();

        public async Task<IActionResult> AllTakenTablesByDate(DateOnly date)
        {
            if (!this.ModelState.IsValid)
            {
                this.TempData[Error] = this.ModelState
                    .Values
                    .First()
                    .Errors
                    .First()
                    .ErrorMessage;

                return RedirectToAction(nameof(Index));
            }

            var tables = await this.reservationService.AllByDate(date);

            return View(new Tuple<DateOnly, IEnumerable<TableServiceModel>>(date, tables));
        }
    }
}

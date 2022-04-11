namespace FoodFun.Web.Controllers
{
    using FoodFun.Core.Contracts;
    using FoodFun.Core.ValidationAttributes.Date;
    using Microsoft.AspNetCore.Mvc;

    using static Constants.GlobalConstants.Messages;

    public class ReservationController : Controller
    {
        private readonly IReservationService reservationService;

        public ReservationController(IReservationService reservationService) 
            => this.reservationService = reservationService;

        public IActionResult Index()
            => View();

        public async Task<IActionResult> FreeTablesForDate([ShouldBeNowOrInTheFuture] DateOnly date)
        {
            if (!this.ModelState.IsValid)
            {
                this.TempData[Error] = this.ModelState.Values.First().Errors.First().ErrorMessage;

                return RedirectToAction(nameof(Index));
            }
            //extract free table for date (return tables)
            var freeTables = await this.reservationService.FreeTables(date);

            return View(freeTables);
        }
    }
}

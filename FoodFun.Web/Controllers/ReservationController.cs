namespace FoodFun.Web.Controllers
{
    using FoodFun.Core.Contracts;
    using FoodFun.Core.Models.Table;
    using FoodFun.Core.ValidationAttributes.Date;
    using FoodFun.Core.ValidationAttributes.Table;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;

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

            var freeTables = await this.reservationService.FreeTables(date);

            return View(new Tuple<DateOnly, IEnumerable<TableServiceModel>>(date, freeTables));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Reserve(
            [ShouldBeNowOrInTheFuture] DateOnly date,
            [ShouldBeExistingTable] string id)
        {
            if (!this.ModelState.IsValid)
            {
                this.TempData[Error] = SomethingWentWrong;

                return RedirectToAction(nameof(Index));
            }

            var isSucceed = await this.reservationService
                .Reserv(date, id, this.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);

            return View();
        }
    }
}

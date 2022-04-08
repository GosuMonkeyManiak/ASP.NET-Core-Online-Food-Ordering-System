namespace FoodFun.Web.Controllers
{
    using FoodFun.Core.ValidationAttributes.Date;
    using Microsoft.AspNetCore.Mvc;

    using static Constants.GlobalConstants.Messages;

    public class ReservationController : Controller
    {
        public IActionResult Index()
            => View();

        public IActionResult FreeTablesForDate([ShouldBeNowOrInTheFuture] DateOnly date)
        {
            if (!this.ModelState.IsValid)
            {
                this.TempData[Error] = this.ModelState.Values.First().Errors.First().ErrorMessage;

                return RedirectToAction(nameof(Index));
            }

            return Ok(date.Year);
        }
    }
}

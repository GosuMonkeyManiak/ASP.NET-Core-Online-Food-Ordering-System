namespace FoodFun.Web.Controllers
{
    using FoodFun.Web.Models.Reservation;
    using Microsoft.AspNetCore.Mvc;

    public class ReservationController : Controller
    {
        public IActionResult Index()
            => View();

        public IActionResult FreeTablesForDate(DateOnly date)
        {
            if (!this.ModelState.IsValid)
            {
                return Ok(this.ModelState.Values);
            }

            return Ok(date.Year);
        }
    }
}

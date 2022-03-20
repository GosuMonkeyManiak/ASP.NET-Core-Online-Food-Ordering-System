﻿namespace FoodFun.Web.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static Constants.GlobalConstants.Roles;
    using static Constants.GlobalConstants.Areas;

    [Area(Administration)]
    [Authorize(Roles = Administrator)]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

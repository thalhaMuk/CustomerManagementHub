﻿using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CustomerManagementHub.Presentation.Controllers.API
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
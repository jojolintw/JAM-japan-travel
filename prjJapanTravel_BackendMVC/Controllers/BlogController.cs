﻿using Microsoft.AspNetCore.Mvc;

namespace prjJapanTravel_BackendMVC.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
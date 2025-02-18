﻿using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using TenkiAme.Models;

namespace TenkiAme.Controllers
{
    public class HomeController : Controller
    {
        private ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        //GET: /Home/
        [Route("/")]
        [Route("Home")]
        [Route("Home/Index")]
        public async Task<IActionResult> Index()
        {
            var model = new WeatherModel();
            await Task.WhenAll(model.InitializeAsync());
            return View(model);
        }

        ////GET: /Home/Welcome
        //[Route("Home/Welcome")]
        //public IActionResult Welcome(string name, int numTimes = 1)
        //{
        //    if (name == null)
        //    {
        //        name = "";
        //    }

        //    string encodedName = HtmlEncoder.Default.Encode(name);

        //    ViewData["Message"] = "Hello " + encodedName;
        //    ViewData["NumTimes"] = numTimes;

        //    return View();
        //}
    }
}

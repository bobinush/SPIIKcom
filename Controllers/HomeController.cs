﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SPIIKcom.Controllers
{
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Kontakt()
		{
			ViewData["Message"] = "Your contact page.";
			return View();
		}

		public IActionResult Error()
		{
			return View();
		}
	}
}

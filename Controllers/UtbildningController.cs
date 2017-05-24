﻿using Microsoft.AspNetCore.Mvc;
using SPIIKcom.Data;

namespace SPIIKcom.Controllers
{
	public class UtbildningController : Controller
	{
		private readonly ApplicationDbContext _db;
		public UtbildningController(ApplicationDbContext context)
		{
			_db = context;
		}
		public IActionResult Index()
		{
			return View();
		}
	}
}

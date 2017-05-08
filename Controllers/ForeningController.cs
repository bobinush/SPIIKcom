﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SPIIKcom.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace SPIIKcom.Controllers
{
	public class ForeningController : Controller
	{
		private readonly ApplicationDbContext db;
		public ForeningController(ApplicationDbContext context)
		{
			db = context;
		}
		public async Task<IActionResult> Om()
		{
			return View();
		}
		public async Task<IActionResult> Styrelse()
		{
			var model = await db.UnionMembers
				.ToListAsync();

			return View(model);
		}
		public async Task<IActionResult> Sexmasteri()
		{
			var model = await db.UnionMembers
				.ToListAsync();

			return View(model);
		}
	}
}

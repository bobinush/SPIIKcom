using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using SPIIKcom.Data;
using SPIIKcom.Filters;
using SPIIKcom.Services;
using SPIIKcom.ViewModels;

namespace SPIIKcom.Controllers
{
	public class IntroduktionController : Controller
	{
		private readonly ApplicationDbContext _db;
		private readonly SpiikService _spiikService;
		public IntroduktionController(ApplicationDbContext context, SpiikService spiikservice)
		{
			_db = context;
			_spiikService = spiikservice;
		}
		public async Task<IActionResult> Index()
		{
			return View(await _db.StaticPages.AsNoTracking().SingleOrDefaultAsync(x => x.Name == "IntroStart"));
		}

		public async Task<IActionResult> BraAttVeta()
		{
			return View(await _db.StaticPages.AsNoTracking().SingleOrDefaultAsync(x => x.Name == "IntroBraAttVeta"));
		}

		public async Task<IActionResult> Schema()
		{
			return View(await _db.StaticPages.AsNoTracking().SingleOrDefaultAsync(x => x.Name == "IntroSchema"));
		}

		public async Task<IActionResult> Stab()
		{
			return View(await _db.StaticPages.AsNoTracking().SingleOrDefaultAsync(x => x.Name == "IntroStab"));
		}

		public async Task<IActionResult> Faddrar()
		{
			return View(await _db.StaticPages.AsNoTracking().SingleOrDefaultAsync(x => x.Name == "IntroFaddrar"));
		}

		public async Task<IActionResult> Kontakt()
		{
			return View(await _db.StaticPages.AsNoTracking().SingleOrDefaultAsync(x => x.Name == "IntroKontakt"));
		}
	}
}

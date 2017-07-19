using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using SPIIKcom.Data;
using SPIIKcom.Enums;
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
			var viewModel = new IntroViewModel
			{
				Text = await _db.StaticPages.AsNoTracking().SingleOrDefaultAsync(x => x.Name == "IntroStab"),
				Personal = await _db.IntroPersonal.AsNoTracking().Where(x => x.IntroType == IntroTypeEnum.Stab).ToListAsync()
			};

			return View(viewModel);
		}

		public async Task<IActionResult> Faddrar()
		{
			var viewModel = new IntroViewModel
			{
				Text = await _db.StaticPages.AsNoTracking().SingleOrDefaultAsync(x => x.Name == "IntroFaddrar"),
				Personal = await _db.IntroPersonal.AsNoTracking().Where(x => x.IntroType == IntroTypeEnum.Fadder).ToListAsync()
			};

			return View(viewModel);
		}

		public async Task<IActionResult> Kontakt()
		{
			return View(await _db.StaticPages.AsNoTracking().SingleOrDefaultAsync(x => x.Name == "IntroKontakt"));
		}
	}
}

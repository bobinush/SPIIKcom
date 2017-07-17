using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SPIIKcom.Data;
using SPIIKcom.Enums;
using SPIIKcom.Extensions;
using SPIIKcom.Filters;
using SPIIKcom.Models;
using SPIIKcom.Models.AccountViewModels;
using SPIIKcom.Services;
using SPIIKcom.ViewModels;

namespace SPIIKcom.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin,Styrelse")]
	public class IntroController : Controller
	{
		private readonly ApplicationDbContext _db;
		private readonly ILogger _logger;
		private IHostingEnvironment _env;
		private readonly SpiikService _spiikService;

		public IntroController(
			ApplicationDbContext context,
			ILoggerFactory loggerFactory,
			IHostingEnvironment env,
			SpiikService spiikService)
		{
			_db = context;
			_logger = loggerFactory.CreateLogger<IntroController>();
			_env = env;
			_spiikService = spiikService;
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			var viewModel = new IntroViewModel();
			viewModel.Text = await _db.StaticPages.AsNoTracking().Where(x => x.Name.StartsWith("Intro")).ToListAsync();
			viewModel.Personal = await _db.IntroPersonal.OrderBy(x => x.IntroType).AsNoTracking().ToListAsync();
			return View(viewModel);
		}

	#region Create, edit and delete personel below.

		[HttpGet]
		public async Task<IActionResult> Create()
		{
			return View(new IntroPersonalViewModel());
		}

		[HttpPost]
		public async Task<IActionResult> Create(IntroPersonalViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				var model = new IntroPersonal(viewModel);
				var webRoot = _env.WebRootPath;
				if (viewModel.Picture != null)
				{
					Tuple<bool, string> result = await _spiikService.SaveFile(viewModel.Picture, "images/IntroPersonal", viewModel.Name);
					if (result.Item1) // Success saving
					{
						if (!string.IsNullOrWhiteSpace(result.Item2)) // Spara endast om en bild har blivit uppladdad.
							model.PictureSrc = result.Item2;
					}
					else
					{
						ModelState.AddModelError("Picture", result.Item2);
						return View();
					}
				}
				await _db.AddAsync(model);
				await _db.SaveChangesAsync();
				TempData["Message"] = "Personal skapad!";
				return RedirectToAction("Index");
			}
			return View(viewModel);
		}

		[HttpGet]
		public async Task<IActionResult> Edit(int id)
		{
			var model = await _db.IntroPersonal.FindAsync(id);
			if (model == null)
				return RedirectToAction("Index");

			var viewModel = new IntroPersonalViewModel(model);
			return View(viewModel);
		}

		[HttpPost]
		[RequestFormSizeLimit(5000000)] // Max image size 5 MB
		public async Task<IActionResult> Edit(IntroPersonalViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				var model = await _db.IntroPersonal.FindAsync(viewModel.Id);
				if (model == null)
					return RedirectToAction("Index");

				model.Name = viewModel.Name;
				model.NickName = viewModel.NickName;
				model.Age = viewModel.Age;
				model.Program = viewModel.Program;
				model.Quote = viewModel.Quote;
				model.Bribe = viewModel.Bribe;
				model.GoodWord = viewModel.GoodWord;
				model.IntroType = viewModel.IntroType;

				if (viewModel.Picture?.Length > 0)
				{
					Tuple<bool, string> result = await _spiikService.SaveFile(viewModel.Picture, "images/IntroPersonal", viewModel.Name);
					if (result.Item1) // Success saving
					{
						if (!string.IsNullOrWhiteSpace(result.Item2)) // Spara endast om en bild har blivit uppladdad.
							model.PictureSrc = result.Item2;
					}
					else
					{
						ModelState.AddModelError("Picture", result.Item2);
						return View(viewModel);
					}
				}

				await _db.SaveChangesAsync();
				TempData["Message"] = "Personal uppdaterad!";
				return RedirectToAction("Index");
			}
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> Delete(int id)
		{
			var model = await _db.IntroPersonal.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);
			if (model == null)
				return RedirectToAction("Index");

			return View(model);
		}

		[HttpPost, ActionName("Delete")]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			if (ModelState.IsValid)
			{
				var model = await _db.IntroPersonal.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);
				if (model == null)
					return RedirectToAction("Index");

				_db.Remove(model);
				await _db.SaveChangesAsync();
				TempData["Message"] = "Personal raderad!";
				return RedirectToAction("Index");
			}
			return View();
		}

	#endregion
	#region Create, edit and delete texts below.

		[HttpGet]
		public async Task<IActionResult> CreateText()
		{
			return View(new StaticPage());
		}

		[HttpPost]
		public async Task<IActionResult> CreateText(StaticPage model)
		{
			if (ModelState.IsValid)
			{
				await _db.AddAsync(model);
				await _db.SaveChangesAsync();
				TempData["Message"] = "Text skapad!";
				return RedirectToAction("Index");
			}
			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> EditText(int id)
		{
			var model = await _db.StaticPages.FindAsync(id);
			if (model == null)
				return RedirectToAction("Index");

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> EditText(StaticPage model)
		{
			if (ModelState.IsValid)
			{
				_db.Attach(model);
				_db.Entry(model);
				_db.Entry(model).State = EntityState.Modified;
				await _db.SaveChangesAsync();
				TempData["Message"] = "Text uppdaterad!";
				return RedirectToAction("Index");
			}
			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> DeleteText(int id)
		{
			var model = await _db.StaticPages.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);
			if (model == null)
				return RedirectToAction("Index");

			return View(model);
		}

		[HttpPost, ActionName("DeleteText")]
		public async Task<IActionResult> DeleteTextConfirmed(int id)
		{
			if (ModelState.IsValid)
			{
				var model = await _db.StaticPages.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);
				if (model == null)
					return RedirectToAction("Index");

				_db.Remove(model);
				await _db.SaveChangesAsync();
				TempData["Message"] = "Text raderad!";
				return RedirectToAction("Index");
			}
			return View();
		}

		#endregion

	}
}

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
	public class UnionMemberController : Controller
	{
		private readonly ApplicationDbContext _db;
		private readonly ILogger _logger;
		private IHostingEnvironment _env;
		private readonly SpiikService _spiikService;

		public UnionMemberController(
			ApplicationDbContext context,
			ILoggerFactory loggerFactory,
			IHostingEnvironment env,
			SpiikService spiikService)
		{
			_db = context;
			_logger = loggerFactory.CreateLogger<UnionMemberController>();
			_env = env;
			_spiikService = spiikService;
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			return View(await _db.UnionMembers.AsNoTracking().ToListAsync());
		}

		[HttpGet]
		public async Task<IActionResult> Create()
		{
			return View(new UnionMemberViewModel { PictureSrc = "SPIIK-logga.png" });
		}

		[HttpPost]
		public async Task<IActionResult> Create(UnionMemberViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				var model = new UnionMember(viewModel);
				var webRoot = _env.WebRootPath;
				if (viewModel.Picture != null)
				{
					Tuple<bool, string> result = await _spiikService.SaveFile(viewModel.Picture, "images/unionmembers", viewModel.Name);
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
				TempData["Message"] = "Medlem skapad!";
				return RedirectToAction("Index");
			}
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> Edit(int id)
		{
			var model = await _db.UnionMembers.FindAsync(id);
			if (model == null)
				return RedirectToAction("Index");

			var viewModel = new UnionMemberViewModel(model);
			return View(viewModel);
		}

		[HttpPost]
		[RequestFormSizeLimit(5000000)] // Max image size 5 MB
		public async Task<IActionResult> Edit(UnionMemberViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				var model = await _db.UnionMembers.FindAsync(viewModel.Id);
				if (model == null)
					return RedirectToAction("Index");

				model.Name = viewModel.Name;
				model.Title = viewModel.Title;
				model.UnionTypes = viewModel.UnionTypes;
				model.Email = viewModel.Email;
				model.Phone = viewModel.Phone;
				model.Quote = viewModel.Quote;

				if (viewModel.Picture?.Length > 0)
				{
					Tuple<bool, string> result = await _spiikService.SaveFile(viewModel.Picture, "images/unionmembers", viewModel.Name);
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
				TempData["Message"] = "Medlem uppdaterad!";
				return RedirectToAction("Index");
			}
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> Delete(int id)
		{
			var model = await _db.UnionMembers.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);
			if (model == null)
				return RedirectToAction("Index");

			return View(model);
		}

		[HttpPost, ActionName("Delete")]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			if (ModelState.IsValid)
			{
				var model = await _db.UnionMembers.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);
				if (model == null)
					return RedirectToAction("Index");

				_db.Remove(model);
				await _db.SaveChangesAsync();
				TempData["Message"] = "Medlem raderad!";
				return RedirectToAction("Index");
			}
			return View();
		}
	}
}

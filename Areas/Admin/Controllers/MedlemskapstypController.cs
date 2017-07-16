using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SPIIKcom.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using SPIIKcom.Models;
using System.Collections.Generic;
using SPIIKcom.ViewModels;
using System;
using System.Net;

namespace SPIIKcom.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin,Styrelse")]
	public class MedlemskapstypController : Controller
	{
		private readonly ApplicationDbContext _db;
		public MedlemskapstypController(ApplicationDbContext context)
		{
			_db = context;
		}
		[HttpGet]
		public async Task<IActionResult> Index()
		{
			return View(await _db.MembershipTypes.AsNoTracking().ToListAsync());
		}

		[HttpGet]
		public async Task<IActionResult> Create()
		{
			return View(new MembershipType());
		}

		[HttpPost]
		public async Task<IActionResult> Create(MembershipType model)
		{
			if (ModelState.IsValid)
			{
				model.Price = Math.Round(model.Price, 2, MidpointRounding.AwayFromZero);
				await _db.AddAsync(model);
				await _db.SaveChangesAsync();
				TempData["Message"] = "Medlemstyp skapad!";
				return RedirectToAction("Index");
			}
			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> Edit(int id)
		{
			var model = await _db.MembershipTypes.FindAsync(id);
			if (model == null)
				return RedirectToAction("Index");

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(MembershipType model)
		{
			if (ModelState.IsValid)
			{
				model.Price = Math.Round(model.Price, 2, MidpointRounding.AwayFromZero);
				_db.MembershipTypes.Attach(model);
				_db.Entry(model);
				_db.Entry(model).State = EntityState.Modified;
				await _db.SaveChangesAsync();
				TempData["Message"] = "Medlemstyp uppdaterad!";
				return RedirectToAction("Index");
			}
			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> Delete(int id)
		{
			var model = await _db.MembershipTypes.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);
			if (model == null)
			{
				TempData["Error"] = (int)HttpStatusCode.NotFound;
				return RedirectToAction(nameof(SPIIKcom.Controllers.HomeController.Error), "Home", new { area = "" });
			}

			return View(model);
		}

		[HttpPost, ActionName("Delete")]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			if (ModelState.IsValid)
			{
				var model = await _db.MembershipTypes.FindAsync(id);
				if (model == null)
				{
					TempData["Error"] = (int)HttpStatusCode.NotFound;
					return RedirectToAction(nameof(SPIIKcom.Controllers.HomeController.Error), "Home", new { area = "" });
				}

				_db.MembershipTypes.Remove(model);
				await _db.SaveChangesAsync();
				TempData["Message"] = "Medlemstyp raderad!";
				return RedirectToAction("Index");
			}
			return View();
		}
	}
}

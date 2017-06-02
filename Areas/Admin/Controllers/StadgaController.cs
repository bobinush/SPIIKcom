using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SPIIKcom.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using SPIIKcom.Models;
using System.Collections.Generic;
using SPIIKcom.ViewModels;
using System.Net;

namespace SPIIKcom.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin,Styrelse")]
	public class StadgaController : Controller
	{
		private readonly ApplicationDbContext _db;
		public StadgaController(ApplicationDbContext context)
		{
			_db = context;
		}
		[HttpGet]
		public async Task<IActionResult> Index()
		{
			return View(await _db.Stadgar.OrderBy(x => x.Id).AsNoTracking().ToListAsync());
		}

		[HttpGet]
		public async Task<IActionResult> Create()
		{
			return View(new Stadga());
		}

		[HttpPost]
		public async Task<IActionResult> Create(Stadga model)
		{
			if (ModelState.IsValid)
			{
				await _db.AddAsync(model);
				await _db.SaveChangesAsync();
				TempData["Message"] = "Stadga skapad!";
				return RedirectToAction("Index");
			}
			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> Edit(int id)
		{
			var model = await _db.Stadgar.FindAsync(id);
			if (model == null)
				return RedirectToAction("Index");

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(Stadga model)
		{
			if (ModelState.IsValid)
			{
				_db.Stadgar.Attach(model);
				_db.Entry(model);
				_db.Entry(model).State = EntityState.Modified;
				await _db.SaveChangesAsync();
				TempData["Message"] = "Stadga uppdaterad!";
				return RedirectToAction("Index");
			}
			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> Delete(int id)
		{
			var model = await _db.Stadgar.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);
			if (model == null)
			{
				TempData["Error"] = (int)HttpStatusCode.NotFound;
				return RedirectToAction("Error", "Home", new { area = "" });
			}

			return View(model);
		}

		[HttpPost, ActionName("Delete")]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			if (ModelState.IsValid)
			{
				var model = await _db.Stadgar.FindAsync(id);
				if (model == null)
				{
					TempData["Error"] = (int)HttpStatusCode.NotFound;
					return RedirectToAction("Error", "Home", new { area = "" });
				}

				_db.Stadgar.Remove(model);
				await _db.SaveChangesAsync();
				TempData["Message"] = "Stadga raderad!";
				return RedirectToAction("Index");
			}
			return View();
		}
	}
}

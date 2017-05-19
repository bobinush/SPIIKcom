using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SPIIKcom.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using SPIIKcom.Models;
using System.Collections.Generic;
using SPIIKcom.ViewModels;

namespace SPIIKcom.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin,Styrelse")]
	public class StadgaController : Controller
	{
		private readonly ApplicationDbContext db;
		public StadgaController(ApplicationDbContext context)
		{
			db = context;
		}
		[HttpGet]
		public async Task<IActionResult> Index()
		{
			return View(await db.Stadgar.OrderBy(x => x.Number).AsNoTracking().ToListAsync());
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
				await db.AddAsync(model);
				await db.SaveChangesAsync();
				TempData["Message"] = "Stadga skapad!";
				return RedirectToAction("Index");
			}
			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> Edit(int id)
		{
			var model = await db.Stadgar.FindAsync(id);
			if (model == null)
				return RedirectToAction("Index");

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(Stadga model)
		{
			if (ModelState.IsValid)
			{
				db.Stadgar.Attach(model);
				db.Entry(model);
				db.Entry(model).State = EntityState.Modified;
				await db.SaveChangesAsync();
				TempData["Message"] = "stadga uppdaterad!";
				return RedirectToAction("Index");
			}
			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> Delete(int id)
		{
			var model = await db.Stadgar.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);
			if (model == null)
				return new StatusCodeResult(404);

			return View(model);
		}

		[HttpPost, ActionName("Delete")]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			if (ModelState.IsValid)
			{
				var model = await db.Stadgar.FindAsync(id);
				if (model == null)
					return new StatusCodeResult(404);

				db.Stadgar.Remove(model);
				await db.SaveChangesAsync();
				TempData["Message"] = "stadga raderad!";
				return RedirectToAction("Index");
			}
			return View();
		}

		[HttpGet]
		public IActionResult StadgaPartial()
		{
			return PartialView("_Stadga", new Stadga());
		}
	}
}

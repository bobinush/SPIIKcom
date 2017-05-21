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

namespace SPIIKcom.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin,Styrelse")]
	public class MedlemskapstypController : Controller
	{
		private readonly ApplicationDbContext db;
		public MedlemskapstypController(ApplicationDbContext context)
		{
			db = context;
		}
		[HttpGet]
		public async Task<IActionResult> Index()
		{
			return View(await db.MembershipTypes.AsNoTracking().ToListAsync());
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
				await db.AddAsync(model);
				await db.SaveChangesAsync();
				TempData["Message"] = "Medlemstyp skapad!";
				return RedirectToAction("Index");
			}
			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> Edit(int id)
		{
			var model = await db.MembershipTypes.FindAsync(id);
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
				db.MembershipTypes.Attach(model);
				db.Entry(model);
				db.Entry(model).State = EntityState.Modified;
				await db.SaveChangesAsync();
				TempData["Message"] = "Medlemstyp uppdaterad!";
				return RedirectToAction("Index");
			}
			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> Delete(int id)
		{
			var model = await db.MembershipTypes.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);
			if (model == null)
				return new StatusCodeResult(404);

			return View(model);
		}

		[HttpPost, ActionName("Delete")]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			if (ModelState.IsValid)
			{
				var model = await db.MembershipTypes.FindAsync(id);
				if (model == null)
					return new StatusCodeResult(404);

				db.MembershipTypes.Remove(model);
				await db.SaveChangesAsync();
				TempData["Message"] = "stadga raderad!";
				return RedirectToAction("Index");
			}
			return View();
		}
	}
}

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
			var viewModel = new StadgaViewModel();
			viewModel.Stadgar = await db.Stadgar
			.AsNoTracking()
			.ToListAsync();

			return View(viewModel);
		}
		[HttpPost]
		public async Task<IActionResult> Index(StadgaViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				foreach (Stadga stadga in viewModel.Stadgar)
				{
					if (!string.IsNullOrWhiteSpace(stadga.Number) ||
						!string.IsNullOrWhiteSpace(stadga.Name) ||
						!string.IsNullOrWhiteSpace(stadga.Text))
					{
						if (stadga.Id == 0)
							await db.Stadgar.AddAsync(stadga);
						else
						{
							db.Stadgar.Attach(stadga);
							var entry = db.Entry(stadga);
							db.Entry(stadga).State = EntityState.Modified;
							// other changed properties
							db.SaveChanges();
						}
					}
				}

				await db.SaveChangesAsync();
				TempData["Message"] = "Stadgar uppdaterade!";
				return RedirectToAction("Index");
			}
			return View(viewModel);
		}
		[HttpGet]
		public IActionResult StadgaPartial(int index)
		{
			return PartialView("_Stadga", new Stadga { Id = index });
		}
	}
}

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SPIIKcom.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using SPIIKcom.ViewModels;

namespace SPIIKcom.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin,Styrelse")]
	public class KontaktController : Controller
	{
		private readonly ApplicationDbContext db;
		public KontaktController(ApplicationDbContext context)
		{
			db = context;
		}
		[HttpGet]
		public async Task<IActionResult> Index()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Edit(ContactPageViewModel viewModel)
		{
			if (ModelState.IsValid)
			{

				await db.SaveChangesAsync();
				TempData["Message"] = "Medlem uppdaterad!";
				return RedirectToAction("Index");
			}
			return View(viewModel);
		}
	}
}

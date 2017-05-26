using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SPIIKcom.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using SPIIKcom.Models;

namespace SPIIKcom.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Route("Admin")]
	[Authorize(Roles = "Admin, Styrelse")]
	public class HomeController : Controller
	{
		private readonly ApplicationDbContext _db;
		public HomeController(ApplicationDbContext context)
		{
			_db = context;
		}
		[HttpGet]
		public async Task<IActionResult> Index()
		{
			return View(await _db.Organization.AsNoTracking().SingleOrDefaultAsync());
		}
		[HttpPost]
		public async Task<IActionResult> Index(Organization model)
		{
			if (ModelState.IsValid)
			{
				_db.Entry(model).State = EntityState.Modified;

				await _db.SaveChangesAsync();
				TempData["Message"] = "Uppgifter uppdaterade!";
				return RedirectToAction("Index");
			}
			return View("Index", model);
		}
	}
}

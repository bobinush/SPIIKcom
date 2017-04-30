using Microsoft.AspNetCore.Mvc;
using SPIIKcom.Data;

namespace SPIIKcom.Controllers
{
	public class ForeningController : Controller
	{
		private readonly ApplicationDbContext db;
		public ForeningController(ApplicationDbContext context)
		{
			db = context;
		}
		public IActionResult Styrelse()
		{
			return View();
		}
		public IActionResult Sexmasteri()
		{
			return View();
		}
	}
}

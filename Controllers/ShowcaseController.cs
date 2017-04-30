using Microsoft.AspNetCore.Mvc;
using SPIIKcom.Data;

namespace SPIIKcom.Controllers
{
	public class ShowcaseController : Controller
	{
		private readonly ApplicationDbContext db;
		public ShowcaseController(ApplicationDbContext context)
		{
			db = context;
		}
		public IActionResult Index()
		{
			return View();
		}
	}
}

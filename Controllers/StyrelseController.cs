using Microsoft.AspNetCore.Mvc;
using SPIIKcom.Data;

namespace SPIIKcom.Controllers
{
	public class StyrelseController : Controller
	{
		private readonly ApplicationDbContext db;
		public StyrelseController(ApplicationDbContext context)
		{
			db = context;
		}
		public IActionResult Index()
		{
			return View();
		}
	}
}

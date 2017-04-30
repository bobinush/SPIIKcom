using Microsoft.AspNetCore.Mvc;
using SPIIKcom.Data;

namespace SPIIKcom.Controllers
{
	public class UtbildningController : Controller
	{
		private readonly ApplicationDbContext db;
		public UtbildningController(ApplicationDbContext context)
		{
			db = context;
		}
		public IActionResult Index()
		{
			return View();
		}
	}
}

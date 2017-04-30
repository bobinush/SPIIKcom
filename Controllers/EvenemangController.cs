using Microsoft.AspNetCore.Mvc;
using SPIIKcom.Data;

namespace SPIIKcom.Controllers
{
	public class EvenemangController : Controller
	{
		private readonly ApplicationDbContext db;
		public EvenemangController(ApplicationDbContext context)
		{
			db = context;
		}
		public IActionResult Index()
		{
			return View();
		}
	}
}

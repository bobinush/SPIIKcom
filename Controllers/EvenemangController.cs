using Microsoft.AspNetCore.Mvc;
using SPIIKcom.Data;

namespace SPIIKcom.Controllers
{
	public class EvenemangController : Controller
	{
		private readonly ApplicationDbContext _db;
		public EvenemangController(ApplicationDbContext context)
		{
			_db = context;
		}
		public IActionResult Index()
		{
			return View();
		}
	}
}

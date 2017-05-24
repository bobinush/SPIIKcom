using Microsoft.AspNetCore.Mvc;
using SPIIKcom.Data;

namespace SPIIKcom.Controllers
{
	public class ShowcaseController : Controller
	{
		private readonly ApplicationDbContext _db;
		public ShowcaseController(ApplicationDbContext context)
		{
			_db = context;
		}
		public IActionResult Index()
		{
			return View();
		}
	}
}

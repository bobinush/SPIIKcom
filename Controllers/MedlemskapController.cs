using Microsoft.AspNetCore.Mvc;
using SPIIKcom.Data;

namespace SPIIKcom.Controllers
{
	public class MedlemskapController : Controller
	{
		private readonly ApplicationDbContext db;
		public MedlemskapController(ApplicationDbContext context)
		{
			db = context;
		}
		public IActionResult Index()
		{
			return View();
		}
	}
}

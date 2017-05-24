using Microsoft.AspNetCore.Mvc;
using SPIIKcom.Data;

namespace SPIIKcom.Controllers
{
	public class MedlemskapController : Controller
	{
		private readonly ApplicationDbContext _db;
		public MedlemskapController(ApplicationDbContext context)
		{
			_db = context;
		}
		public IActionResult Index()
		{
			return View();
		}
	}
}

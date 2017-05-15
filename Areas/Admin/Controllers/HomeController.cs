using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SPIIKcom.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace SPIIKcom.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Route("admin")]
	[Authorize(Roles = "Admin,Styrelse")]
	public class HomeController : Controller
	{
		private readonly ApplicationDbContext db;
		public HomeController(ApplicationDbContext context)
		{
			db = context;
		}
		public async Task<IActionResult> Index()
		{
			return View();
		}
	}
}

using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPIIKcom.Data;

namespace SPIIKcom.Controllers
{
	public class MemberController : Controller
	{
		private readonly ApplicationDbContext _db;
		public MemberController(ApplicationDbContext context)
		{
			_db = context;
		}
		public async Task<IActionResult> Index()
		{
			var allMembers = await _db.Members.ToListAsync();
			return View(allMembers);
		}
	}
}

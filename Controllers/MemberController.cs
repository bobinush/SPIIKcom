using System;
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

		public async Task<IActionResult> Index(string sort, string name)
		{
			ViewData["FNameSortParam"] = string.IsNullOrEmpty(sort) ? "fNameDesc" : "";
			ViewData["LNameSortParam"] = sort == "lNameDesc" ? "lName" : "lNameDesc";
			ViewData["JoinDateSortParam"] = sort == "joinDateDesc" ? "joinDate" : "joinDateDesc";
			ViewData["ExpDateSortParam"] = sort == "expDateDesc" ? "expDate" : "expDateDesc";
			ViewData["CurrentFilterName"] = name;

			var allMembers = from m in _db.Members select m;

			if (!string.IsNullOrWhiteSpace(name))
			{
				allMembers = allMembers.Where(m => m.Name.IndexOf(name, StringComparison.OrdinalIgnoreCase) > -1
												|| m.LastName.IndexOf(name, StringComparison.OrdinalIgnoreCase) > -1);
			}

			switch (sort)
			{
				case "fNameDesc":
					allMembers = allMembers.OrderByDescending(m => m.Name);
					break;
				case "lName":
					allMembers = allMembers.OrderBy(m => m.LastName);
					break;
				case "lNameDesc":
					allMembers = allMembers.OrderByDescending(m => m.LastName);
					break;
				case "joinDateDesc":
					allMembers = allMembers.OrderByDescending(m => m.JoinDate);
					break;
				case "joinDate":
					allMembers = allMembers.OrderBy(m => m.JoinDate);
					break;
				case "expDateDesc":
					allMembers = allMembers.OrderByDescending(m => m.ExpireDate);
					break;
				case "expDate":
					allMembers = allMembers.OrderBy(m => m.ExpireDate);
					break;
				default:
					allMembers = allMembers.OrderBy(m => m.Name);
					break;
			}


			return View(await allMembers.AsNoTracking().ToListAsync());
		}
	}
}

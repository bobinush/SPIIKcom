using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SPIIKcom.Data;
using SPIIKcom.Models;
using SPIIKcom.ViewModels;

namespace SPIIKcom.Controllers
{
	public class MedlemmarController : Controller
	{
		private readonly ApplicationDbContext db;
		public MedlemmarController(ApplicationDbContext context)
		{
			db = context;
		}

		public async Task<IActionResult> Index(string sort, string name)
		{
			ViewData["FirstNameSort"] = string.IsNullOrEmpty(sort) ? "FirstNameDesc" : "";
			ViewData["LastNameSort"] = sort == "LastNameDesc" ? "LastName" : "LastNameDesc";
			ViewData["JoinDateSort"] = sort == "JoinDateDesc" ? "JoinDate" : "JoinDateDesc";
			ViewData["ExpireDateSort"] = sort == "ExpireDateDesc" ? "ExpireDate" : "ExpireDateDesc";
			ViewData["CurrentFilterName"] = name;

			var allMembers = db.Members.AsQueryable();

			if (!string.IsNullOrWhiteSpace(name))
			{
				allMembers = allMembers.Where(m => m.FirstName.IndexOf(name, StringComparison.OrdinalIgnoreCase) > -1
												|| m.LastName.IndexOf(name, StringComparison.OrdinalIgnoreCase) > -1);
				// var filter = Builders<BsonDocument>.Filter.Matches("FirstName", name);
			}

			bool descending = false;
			if (sort.EndsWith("desc"))
			{
				sort = sort.Substring(0, sort.Length - 4);
				descending = true;
			}

			// Hitta kolumnen med hjälp av en sträng med kolumnens namn.
			if (descending)
				allMembers = allMembers.OrderByDescending(e => EF.Property<object>(e, sort));
			else
				allMembers = allMembers.OrderBy(e => EF.Property<object>(e, sort));

			// switch (sort)
			// {
			// 	case "fNameDesc":
			// 		allMembers = allMembers.OrderByDescending(m => m.FirstName);
			// 		break;
			// 	case "lName":
			// 		allMembers = allMembers.OrderBy(m => m.LastName);
			// 		break;
			// 	case "lNameDesc":
			// 		allMembers = allMembers.OrderByDescending(m => m.LastName);
			// 		break;
			// 	case "joinDateDesc":
			// 		allMembers = allMembers.OrderByDescending(m => m.JoinDate);
			// 		break;
			// 	case "joinDate":
			// 		allMembers = allMembers.OrderBy(m => m.JoinDate);
			// 		break;
			// 	case "expDateDesc":
			// 		allMembers = allMembers.OrderByDescending(m => m.ExpireDate);
			// 		break;
			// 	case "expDate":
			// 		allMembers = allMembers.OrderBy(m => m.ExpireDate);
			// 		break;
			// 	default:
			// 		allMembers = allMembers.OrderBy(m => m.FirstName);
			// 		break;
			// }
			return View(await allMembers.AsNoTracking().ToListAsync());
		}

		// TODO : Add the role Styrelse
		[HttpGet]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Create()
		{
			var model = new CreateMemberViewModel();
			var membershipTypes = await db.MembershipTypes.ToListAsync();
			var dict = new Dictionary<double, string>();
			dict.Add(-1, "Välj typ av medlemskap");
			for (int i = 0; i < membershipTypes.Count; i++)
			{
				dict.Add(membershipTypes[i].ID, membershipTypes[i].Price + ":- | " + membershipTypes[i].Name);
			}
			var selectList = new SelectList(dict, "Key", "Value", selectedValue: -1);
			model.MembershipTypes = selectList;
			return View(model);
		}

		[HttpPost]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Create(CreateMemberViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				var membershipType = await db.MembershipTypes.FindAsync((int)viewModel.MembershipTypeId);
				var member = new Member(viewModel);

				member.ExpireDate = member.JoinDate.AddYears(membershipType.LengthInYears);

				await db.AddAsync(member);
				await db.SaveChangesAsync();
				return RedirectToAction("Index");

				// TODO : Postback Selectlist MembershipTypes so we can return the viewmodel if error.
			}
			return View(viewModel);
		}
	}
}

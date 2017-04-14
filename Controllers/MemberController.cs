using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SPIIKcom.Data;
using SPIIKcom.Models;
using SPIIKcom.ViewModels;

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

		[HttpGet]
		public async Task<IActionResult> Create()
		{
			// var model = new CreateMemberViewModel();
			// model.MembershipTypes = _db.MembershipTypes
			// 	.Select(x => new SelectListItem
			// 	{
			// 		Text = x.Price + ":- | " + x.Name,
			// 		Value = x.ID.ToString()
			// 	})
			// 		.ToList();
			// model.MembershipTypes.Add((new SelectListItem{Text="Välj typ av medlemskap", Value = "-1"});
			// return View(model);
			var model = new CreateMemberViewModel();
			var membershipTypes = await _db.MembershipTypes.ToListAsync();
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
		public async Task<IActionResult> Create(CreateMemberViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				var membershipType = await _db.MembershipTypes.FindAsync((int)viewModel.MembershipTypeId);
				var member = new Member(viewModel);
				int daysInYear = 365;
				double membershipDays = Math.Ceiling(daysInYear * membershipType.LengthInYears);

				member.ExpireDate = member.JoinDate.AddDays(membershipDays);

				await _db.AddAsync(member);
				await _db.SaveChangesAsync();
				return RedirectToAction("Index");

				// TODO : Postback Selectlist MembershipTypes so we can return the viewmodel if error.
			}
			return View(viewModel);
		}
	}
}

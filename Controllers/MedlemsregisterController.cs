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
	[Authorize(Roles = "Admin,Styrelse")]
	public class MedlemsregisterController : Controller
	{
		private readonly ApplicationDbContext db;
		public MedlemsregisterController(ApplicationDbContext context)
		{
			db = context;
		}

		public async Task<IActionResult> Index(string sort = "", string name = "")
		{
			ViewData["FirstNameSort"] = string.IsNullOrWhiteSpace(sort) ? "FirstNameDesc" : "";
			ViewData["LastNameSort"] = sort == "LastNameDesc" ? "LastName" : "LastNameDesc";
			ViewData["JoinDateSort"] = sort == "JoinDateDesc" ? "JoinDate" : "JoinDateDesc";
			ViewData["ExpireDateSort"] = sort == "ExpireDateDesc" ? "ExpireDate" : "ExpireDateDesc";
			ViewData["CurrentFilterName"] = name;

			var model = db.Members.AsQueryable();

			if (!string.IsNullOrWhiteSpace(name))
			{
				model = model.Where(m => m.FirstName.IndexOf(name, StringComparison.OrdinalIgnoreCase) > -1
								|| m.LastName.IndexOf(name, StringComparison.OrdinalIgnoreCase) > -1);
			}
			if (string.IsNullOrWhiteSpace(sort))
			{
				model = model.OrderBy(m => m.FirstName);
			}
			else
			{
				bool descending = false;
				if (sort?.EndsWith("desc", StringComparison.OrdinalIgnoreCase) == true)
				{
					sort = sort.Substring(0, sort.Length - 4);
					descending = true;
				}
				// Find the column based on a string with the columname.
				if (descending)
					model = model.OrderByDescending(e => EF.Property<object>(e, sort));
				else
					model = model.OrderBy(e => EF.Property<object>(e, sort));

			}


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
			return View(await model.AsNoTracking().ToListAsync());
		}

		// TODO : Add the role Styrelse
		[HttpGet]
		public async Task<IActionResult> Create()
		{
			var viewModel = new CreateMemberViewModel();
			viewModel.MembershipTypes = await GetMembershipTypes("Välj typ av medlemskap");
			return View(viewModel);
		}

		[HttpPost]
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
			viewModel.MembershipTypes = await GetMembershipTypes("Välj typ av medlemskap");
			return View(viewModel);
		}

		[HttpGet]
		public async Task<IActionResult> Edit(int id)
		{
			var model = await db.Members.FindAsync(id);
			if (model == null)
				return RedirectToAction("Index");

			var viewModel = new EditMemberViewModel(model);
			viewModel.MembershipTypes = await GetMembershipTypes("Uppdatera medlemskapet med");
			return View(viewModel);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(EditMemberViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				var membershipType = await db.MembershipTypes.FindAsync((int)viewModel.MembershipTypeId);


				// TODO : Postback Selectlist MembershipTypes so we can return the viewmodel if error.

				var model = await db.Members.FindAsync(viewModel.Id);
				if (model == null)
					return RedirectToAction("Index");

				model.Id = viewModel.Id;
				model.PersonalNumber = viewModel.PersonalNumber;
				model.FirstName = viewModel.FirstName;
				model.LastName = viewModel.LastName;
				model.Address = viewModel.Address;
				model.PostalCode = viewModel.PostalCode;
				model.City = viewModel.City;
				model.Email = viewModel.Email;
				model.Phone = viewModel.Phone;
				model.Program = viewModel.Program;
				if (viewModel.MembershipTypeId > 0)
				{
					if (model.ExpireDate < DateTime.Today)
						model.ExpireDate = DateTime.Today.AddYears(membershipType.LengthInYears);
					else
						model.ExpireDate = model.ExpireDate.AddYears(membershipType.LengthInYears);
				}

				await db.SaveChangesAsync();
				return RedirectToAction("Index");
			}
			return View(viewModel);
		}
		//
		// GET: /Users/Delete/5
		[HttpGet]
		public async Task<IActionResult> Delete(string id)
		{
			if (id == null)
				return new StatusCodeResult(400); // BadRequest

			var model = await db.Members.FindAsync(id);
			if (model == null)
				return new StatusCodeResult(404);

			return View(model);
		}

		//
		// POST: /Users/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(string id)
		{
			if (ModelState.IsValid)
			{
				if (id == null)
					return new StatusCodeResult(400); // BadRequest

				var model = await db.Members.FindAsync(id);
				if (model == null)
					return new StatusCodeResult(404);

				db.Members.Remove(model);
				await db.SaveChangesAsync();
				return RedirectToAction("Index");
			}
			return View();
		}

		internal async Task<SelectList> GetMembershipTypes(string defaultText)
		{
			var membershipTypes = await db.MembershipTypes.ToListAsync();
			var dict = new Dictionary<double, string>();
			dict.Add(-1, defaultText);
			for (int i = 0; i < membershipTypes.Count; i++)
			{
				dict.Add(membershipTypes[i].Id, membershipTypes[i].Price + ":- | " + membershipTypes[i].Name);
			}
			return new SelectList(dict, "Key", "Value", selectedValue: -1);
		}
	}
}

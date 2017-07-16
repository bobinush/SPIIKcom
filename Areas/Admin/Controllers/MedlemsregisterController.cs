using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SPIIKcom.Data;
using SPIIKcom.Models;
using SPIIKcom.ViewModels;

namespace SPIIKcom.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin,Styrelse")]
	public class MedlemsregisterController : Controller
	{
		private readonly ApplicationDbContext _db;
		public MedlemsregisterController(ApplicationDbContext context)
		{
			_db = context;
		}

		public async Task<IActionResult> Index(string sort = "", string name = "")
		{
			ViewData["FirstNameSort"] = string.IsNullOrWhiteSpace(sort) ? "FirstName" : "FirstNameDesc";
			ViewData["LastNameSort"] = sort == "LastName" ? "LastNameDesc" : "LastName";
			ViewData["JoinDateSort"] = sort == "JoinDate" ? "JoinDateDesc" : "JoinDate";
			ViewData["ExpireDateSort"] = sort == "ExpireDate" ? "ExpireDateDesc" : "ExpireDate";
			ViewData["CurrentFilterName"] = name;

			// TODO : Pagedlist?
			var model = _db.Members.AsQueryable();
			// Search on first/lastname
			if (!string.IsNullOrWhiteSpace(name))
				model = model.Where(m => m.FirstName.IndexOf(name, StringComparison.OrdinalIgnoreCase) > -1
									|| m.LastName.IndexOf(name, StringComparison.OrdinalIgnoreCase) > -1);

			// Default sort on firstname
			if (string.IsNullOrWhiteSpace(sort))
			{
				model = model.OrderBy(m => m.FirstName);
			}
			else
			{
				bool descending = false;
				if (sort?.EndsWith("desc", StringComparison.OrdinalIgnoreCase) == true)
				{
					// Remove "desc" from the string
					sort = sort.Substring(0, sort.Length - 4);
					descending = true;
				}
				// Find the column based on a string with the columname
				if (descending)
					model = model.OrderByDescending(e => EF.Property<object>(e, sort));
				else
					model = model.OrderBy(e => EF.Property<object>(e, sort));
			}

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
				var membershipType = await _db.MembershipTypes.FindAsync((int)viewModel.MembershipTypeId);
				var member = new Member(viewModel);

				member.ExpireDate = member.JoinDate.AddYears(membershipType.LengthInYears);

				await _db.AddAsync(member);
				await _db.SaveChangesAsync();
				TempData["Message"] = "Medlem registrerad!";
				return RedirectToAction("Index");
			}
			viewModel.MembershipTypes = await GetMembershipTypes("Välj typ av medlemskap");
			return View(viewModel);
		}

		[HttpGet]
		public async Task<IActionResult> Edit(int id)
		{
			var model = await _db.Members.FindAsync(id);
			if (model == null)
				return RedirectToAction("Index");

			var viewModel = new EditMemberViewModel(model);
			viewModel.MembershipTypes = await GetMembershipTypes("Uppdatera medlemskapet med");
			if (viewModel.ExpireDate < DateTime.Today)
				viewModel.ExpireStatus = "danger";
			else if (viewModel.ExpireDate < DateTime.Today.AddMonths(1))
				viewModel.ExpireStatus = "warning";
			else
				viewModel.ExpireStatus = "normal";
			return View(viewModel);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(EditMemberViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				var membershipType = await _db.MembershipTypes.FindAsync((int)viewModel.MembershipTypeId);

				var model = await _db.Members.FindAsync(viewModel.Id);
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
				// Om förnyelse/öka medlemskapet
				if (viewModel.MembershipTypeId > 0)
				{
					// Har det tidigare medlemskapet gått ut när man lägger till så ta från dagens datum.
					// Annars så läggs perioden på det redan aktiva medlemskapet.
					if (model.ExpireDate < DateTime.Today)
						model.ExpireDate = DateTime.Today.AddYears(membershipType.LengthInYears);
					else
						model.ExpireDate = model.ExpireDate.AddYears(membershipType.LengthInYears);
				}

				await _db.SaveChangesAsync();
				TempData["Message"] = "Medlem uppdaterad!";
				return RedirectToAction("Index");
			}
			viewModel.MembershipTypes = await GetMembershipTypes("Uppdatera medlemskapet med");
			return View(viewModel);
		}

		[HttpGet]
		public async Task<IActionResult> Delete(int id)
		{
			var model = await _db.Members.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);
			if (model == null)
			{
				TempData["Error"] = (int)HttpStatusCode.NotFound;
				return RedirectToAction(nameof(SPIIKcom.Controllers.HomeController.Error), "Home", new { area = "" });
			}

			return View(model);
		}

		[HttpPost, ActionName("Delete")]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			if (ModelState.IsValid)
			{
				var model = await _db.Members.FindAsync(id);
				if (model == null)
				{
					TempData["Error"] = (int)HttpStatusCode.NotFound;
					return RedirectToAction(nameof(SPIIKcom.Controllers.HomeController.Error), "Home", new { area = "" });
				}

				_db.Members.Remove(model);
				await _db.SaveChangesAsync();
				TempData["Message"] = "Medlem raderad!";
				return RedirectToAction("Index");
			}
			return View();
		}

		internal async Task<SelectList> GetMembershipTypes(string defaultText)
		{
			var membershipTypes = await _db.MembershipTypes.ToListAsync();
			var dict = new Dictionary<double, string>();
			dict.Add(-1, defaultText);
			for (int i = 0; i < membershipTypes.Count; i++)
			{
				dict.Add(membershipTypes[i].Id, membershipTypes[i].Name +
					" (" + membershipTypes[i].LengthInYears + " år, " + membershipTypes[i].Price + "kr)");
			}
			return new SelectList(dict, "Key", "Value", selectedValue: -1);
		}
	}
}

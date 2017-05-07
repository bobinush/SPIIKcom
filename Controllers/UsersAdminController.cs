using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SPIIKcom.Data;
using SPIIKcom.Enums;
using SPIIKcom.Extensions;
using SPIIKcom.Models;
using SPIIKcom.Models.AccountViewModels;
using SPIIKcom.Services;
using SPIIKcom.ViewModels;

namespace SPIIKcom.Controllers
{
	// TODO : Add role Styrelse
	[Authorize(Roles = "Admin")]
	public class UsersAdminController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly IEmailSender _emailSender;
		private readonly ISmsSender _smsSender;
		private readonly ILogger _logger;

		public UsersAdminController(
			UserManager<ApplicationUser> userManager,
			RoleManager<IdentityRole> roleManager,
			SignInManager<ApplicationUser> signInManager,
			IEmailSender emailSender,
			ISmsSender smsSender,
			ILoggerFactory loggerFactory)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_signInManager = signInManager;
			_emailSender = emailSender;
			_smsSender = smsSender;
			_logger = loggerFactory.CreateLogger<UsersAdminController>();
		}

		//
		// GET: /Users/
		public async Task<ActionResult> Index(string name)
		{
			ViewData["CurrentFilterName"] = name;
			var model = _userManager.Users;
			if (!String.IsNullOrEmpty(name))
			{
				model = model.Where(user => user.UserName.Contains(name));
			}

			return View(await model.ToListAsync());
		}

		//
		// GET: /Users/Create
		public async Task<ActionResult> Create()
		{
			//Get the list of Roles as a list with SelectListItems
			var items = Enum.GetValues(typeof(BoardType))
				.Cast<BoardType>()
				.Where(x => x > 0)
				.Select(x => new SelectListItem()
				{
					Text = x.GetDisplayName(),
					Value = ((int)x).ToString()
				}).ToList();

			var model = new RegisterViewModel{
				RolesList = new SelectList(items, "Value", "Text")
			};
			return View(model);
		}

		//
		// POST: /Users/Create
		[HttpPost]
		public async Task<ActionResult> Create(RegisterViewModel userViewModel, params string[] selectedRoles)
		{
			var roles = Enum.GetValues(typeof(BoardType)).Cast<BoardType>();
			ViewData["Roles"] = new SelectList(roles, "Name", "Name");
			if (ModelState.IsValid)
			{
				var user = new ApplicationUser { UserName = userViewModel.Email, Email = userViewModel.Email };
				var adminresult = await _userManager.CreateAsync(user, userViewModel.Password);

				//Add User to the selected Roles
				if (adminresult.Succeeded)
				{
					if (selectedRoles != null)
					{
						var result = await _userManager.AddToRolesAsync(user, selectedRoles);
						if (!result.Succeeded)
						{
							ModelState.AddModelError(string.Empty, result.Errors.First().Description);
							// ViewData["Roles"] = new SelectList(await _roleManager.Roles.ToListAsync(), "Name", "Name");
							return View();
						}
					}
				}
				else
				{
					ModelState.AddModelError(string.Empty, adminresult.Errors.First().Description);
					// ViewData["Roles"] = new SelectList(_roleManager.Roles, "Name", "Name");
					return View();

				}
				return RedirectToAction("Index");
			}
			// ViewData["Roles"] = new SelectList(_roleManager.Roles, "Name", "Name");
			return View();
		}

		//
		// GET: /Users/Edit/1
		public async Task<ActionResult> Edit(string id)
		{
			if (id == null)
				return RedirectToAction("Index");

			var user = await _userManager.FindByIdAsync(id);
			if (user == null)
				return RedirectToAction("Index");

			var userRoles = await _userManager.GetRolesAsync(user);
			var model = new EditUserViewModel()
			{
				Id = user.Id,
				Email = user.Email,
				RolesList = _roleManager.Roles.ToList().Select(x => new SelectListItem()
				{
					Selected = userRoles.Contains(x.Name),
					Text = x.Name,
					Value = x.Name
				})
			};
			return View(model);
		}

		//
		// POST: /Users/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit(EditUserViewModel editUser, params string[] selectedRole)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByIdAsync(editUser.Id);
				if (user == null)
					return RedirectToAction("Index");

				user.UserName = editUser.Email;
				user.Email = editUser.Email;

				var userRoles = await _userManager.GetRolesAsync(user);

				selectedRole = selectedRole ?? new string[] { };

				var result = await _userManager.AddToRolesAsync(user, selectedRole.Except(userRoles).ToList<string>());

				if (!result.Succeeded)
				{
					ModelState.AddModelError(string.Empty, result.Errors.First().Description);
					return View();
				}
				result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRole).ToList<string>());

				if (!result.Succeeded)
				{
					ModelState.AddModelError(string.Empty, result.Errors.First().Description);
					return View();
				}
				return RedirectToAction("Index");
			}
			ModelState.AddModelError(string.Empty, "Something failed.");
			return View();
		}

		//
		// GET: /Users/Delete/5
		public async Task<ActionResult> Delete(string id)
		{
			if (id == null)
				return RedirectToAction("Index");

			var user = await _userManager.FindByIdAsync(id);
			if (user == null)
				return RedirectToAction("Index");

			return View(user);
		}

		//
		// POST: /Users/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> DeleteConfirmed(string id)
		{
			if (ModelState.IsValid)
			{
				if (id == null)
					return RedirectToAction("Index");

				var user = await _userManager.FindByIdAsync(id);
				if (user == null)
					return RedirectToAction("Index");

				var result = await _userManager.DeleteAsync(user);
				if (!result.Succeeded)
				{
					ModelState.AddModelError(string.Empty, result.Errors.First().Description);
					return View();
				}
				return RedirectToAction("Index");
			}
			return View();
		}
	}
}

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

namespace SPIIKcom.Areas.Admin.Controllers
{
	[Area("Admin")]
	// TODO : Add role Styrelse
	[Authorize(Roles = "Admin")]
	public class UsersController : Controller
	{
		private readonly ApplicationDbContext db;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly IEmailSender _emailSender;
		private readonly ISmsSender _smsSender;
		private readonly ILogger _logger;

		public UsersController(
			ApplicationDbContext context,
			UserManager<ApplicationUser> userManager,
			RoleManager<IdentityRole> roleManager,
			SignInManager<ApplicationUser> signInManager,
			IEmailSender emailSender,
			ISmsSender smsSender,
			ILoggerFactory loggerFactory)
		{
			db = context;
			_userManager = userManager;
			_roleManager = roleManager;
			_signInManager = signInManager;
			_emailSender = emailSender;
			_smsSender = smsSender;
			_logger = loggerFactory.CreateLogger<UnionMemberController>();
		}

		//
		// GET: /Users/
		public async Task<IActionResult> Index()
		{
			var model = await _userManager.Users.Include(x => x.Roles).ToListAsync();
			return View(model);
		}

		//
		// GET: /Users/Create
		public async Task<IActionResult> Create()
		{
			ViewData["RolesList"] = _roleManager.Roles.ToList().Select(x => new SelectListItem()
			{
				Text = x.Name,
				Value = x.Name
			});
			return View();
		}

		//
		// POST: /Users/Create
		[HttpPost]
		public async Task<IActionResult> Create(RegisterViewModel viewModel, params string[] selectedRoles)
		{
			ViewData["RolesList"] = _roleManager.Roles.ToList().Select(x => new SelectListItem()
			{
				Text = x.Name,
				Value = x.Name
			});
			if (ModelState.IsValid)
			{
				var user = new ApplicationUser { UserName = viewModel.Email, Email = viewModel.Email, SignupDate = DateTime.Today };
				var adminresult = await _userManager.CreateAsync(user, viewModel.Password);

				//Add User to the selected Roles
				if (adminresult.Succeeded)
				{
					if (selectedRoles != null)
					{
						var result = await _userManager.AddToRolesAsync(user, selectedRoles);
						if (!result.Succeeded)
						{
							ModelState.AddModelError("", result.Errors.First().Description);
							return View();
						}
					}
				}
				else
				{
					ModelState.AddModelError("", adminresult.Errors.First().Description);
					return View();
				}
				return RedirectToAction("Index");
			}
			return View();
		}

		//
		// GET: /Users/Edit/1
		public async Task<IActionResult> Edit(string id)
		{
			ViewData["RolesList"] = _roleManager.Roles.ToList().Select(x => new SelectListItem()
			{
				Text = x.Name,
				Value = x.Name
			});
			if (id == null)
				return new StatusCodeResult(400); // BadRequest

			var user = await _userManager.FindByIdAsync(id);
			if (user == null)
				return new StatusCodeResult(404);

			var userRoles = await _userManager.GetRolesAsync(user);

			var model = new EditUserViewModel()
			{
				Id = user.Id,
				Email = user.Email,
				Name = user.Name,
			};
			return View(model);
		}

		//
		// POST: /Users/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(EditUserViewModel viewModel)//, params string[] selectedRole)
		{
			ViewData["RolesList"] = _roleManager.Roles.ToList().Select(x => new SelectListItem()
			{
				Text = x.Name,
				Value = x.Name
			});
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByIdAsync(viewModel.Id);
				if (user == null)
					return new StatusCodeResult(404);

				user.UserName = viewModel.Email;
				user.Email = viewModel.Email;

				var userRoles = await _userManager.GetRolesAsync(user);

				viewModel.SelectedRoles = viewModel.SelectedRoles ?? new string[] { };

				var result = await _userManager.AddToRolesAsync(user, viewModel.SelectedRoles.Except(userRoles).ToList<string>());

				if (!result.Succeeded)
				{
					ModelState.AddModelError("", result.Errors.First().Description);
					return View();
				}
				result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(viewModel.SelectedRoles).ToList<string>());

				if (!result.Succeeded)
				{
					ModelState.AddModelError("", result.Errors.First().Description);
					return View();
				}
				return RedirectToAction("Index");
			}
			ModelState.AddModelError("", "Something failed.");
			return View();
		}

		//
		// GET: /Users/Delete/5
		public async Task<IActionResult> Delete(string id)
		{
			if (id == null)
				return new StatusCodeResult(400); // BadRequest

			var user = await _userManager.FindByIdAsync(id);
			if (user == null)
				return new StatusCodeResult(404);
			return View(user);
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

				var user = await _userManager.FindByIdAsync(id);
				if (user == null)
					return new StatusCodeResult(404);

				var result = await _userManager.DeleteAsync(user);
				if (!result.Succeeded)
				{
					ModelState.AddModelError("", result.Errors.First().Description);
					return View();
				}
				return RedirectToAction("Index");
			}
			return View();
		}
	}
}

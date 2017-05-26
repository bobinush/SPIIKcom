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
	[Authorize(Roles = "Admin, Styrelse")]
	public class UsersController : Controller
	{
		private readonly ApplicationDbContext _db;
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
			_db = context;
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
			var model = await _userManager.Users.ToListAsync();
			return View(model);
		}

		//
		// GET: /Users/Create
		public async Task<IActionResult> Create()
		{
			var viewModel = new RegisterUserViewModel
			{
				RolesList = _roleManager.Roles.ToList().Select(x => new SelectListItem()
				{
					Text = x.Name,
					Value = x.Name
				})
			};
			return View(viewModel);
		}

		//
		// POST: /Users/Create
		[HttpPost]
		public async Task<IActionResult> Create(RegisterUserViewModel viewModel)
		{
			viewModel.RolesList = _roleManager.Roles.ToList().Select(x => new SelectListItem()
			{
				Selected = viewModel.SelectedRoles.Contains(x.Name), // För att få de valda roller valda igen.
				Text = x.Name,
				Value = x.Name
			});
			if (ModelState.IsValid)
			{
				var user = new ApplicationUser { UserName = viewModel.Email, Email = viewModel.Email, SignupDate = DateTime.Today, Name = viewModel.Name };
				var adminresult = await _userManager.CreateAsync(user, viewModel.Password);

				//Add User to the selected Roles
				if (adminresult.Succeeded)
				{
					if (viewModel.SelectedRoles != null)
					{
						var result = await _userManager.AddToRolesAsync(user, viewModel.SelectedRoles);
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
				TempData["Message"] = "Användare registrerad!";
				return RedirectToAction("Index");
			}
			return View();
		}

		//
		// GET: /Users/Edit/1
		public async Task<IActionResult> Edit(string id)
		{
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
		public async Task<IActionResult> Edit(EditUserViewModel viewModel)
		{
			viewModel.RolesList = _roleManager.Roles.ToList().Select(x => new SelectListItem()
			{
				Selected = viewModel.SelectedRoles.Contains(x.Name), // För att få de valda roller valda igen.
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
				user.Name = viewModel.Name;
				var userRoles = await _userManager.GetRolesAsync(user);
				if (userRoles.Contains("Admin") && !User.IsInRole("Admin")) // Förhindra att någon annan än admin ändrar admin
					return new StatusCodeResult(403); // Forbidden

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
				TempData["Message"] = "Användare uppdaterad!";
				return RedirectToAction("Index");
			}
			ModelState.AddModelError("", "Something failed.");
			return View(viewModel);
		}

		//
		// GET: /Users/Delete/5
		[Authorize(Roles = "Admin")] // Förhindra att någon tar bort admin
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
		[Authorize(Roles = "Admin")] // Förhindra att någon tar bort admin
		[HttpPost, ActionName("Delete")]
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
				TempData["Message"] = "Användare raderad!";
				return RedirectToAction("Index");
			}
			return View();
		}
	}
}

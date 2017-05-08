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
	public class UnionMemberController : Controller
	{
		private readonly ApplicationDbContext db;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly IEmailSender _emailSender;
		private readonly ISmsSender _smsSender;
		private readonly ILogger _logger;

		public UnionMemberController(
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
		[HttpGet]
		public async Task<ActionResult> Index()
		{
			return View(await db.UnionMembers.ToListAsync());
		}

		//
		// GET: /Users/Create
		[HttpGet]		
		public async Task<ActionResult> Create()
		{
			var model = new UnionMemberViewModel();
			return View(model);
		}

		//
		// POST: /Users/Create
		[HttpPost]
		public async Task<ActionResult> Create(UnionMemberViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				var model = new UnionMember(viewModel);
				await db.UnionMembers.AddAsync(model);
				await db.SaveChangesAsync();
				return RedirectToAction("Index");
			}
			return View();
		}

		//
		// GET: /Users/Edit/1
		[HttpGet]		
		public async Task<ActionResult> Edit(int id)
		{
			var model = await db.UnionMembers.FindAsync(id);
			if (model == null)
				return RedirectToAction("Index");

			var viewModel = new UnionMemberViewModel(model);
			return View(viewModel);
		}

		//
		// POST: /Users/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit(UnionMemberViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				var model = await db.UnionMembers.FindAsync(viewModel.Id);
				if (model == null)
					return RedirectToAction("Index");

				model.Id = viewModel.Id;
				model.Name = viewModel.Name;
				model.Title = viewModel.Title;
				model.UnionTypes = viewModel.UnionTypes;
				model.Email = viewModel.Email;
				model.Quote = viewModel.Quote;
				model.PictureSrc = viewModel.PictureSrc;
				await db.SaveChangesAsync();
				return RedirectToAction("Index");
			}
			ModelState.AddModelError(string.Empty, "Something failed.");
			return View();
		}

		//
		// GET: /Users/Delete/
		[HttpGet]		
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

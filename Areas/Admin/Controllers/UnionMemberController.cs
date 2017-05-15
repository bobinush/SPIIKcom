using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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
	[Authorize(Roles = "Admin,Styrelse")]
	public class UnionMemberController : Controller
	{
		private readonly ApplicationDbContext db;
		private readonly ILogger _logger;
		private IHostingEnvironment _env;

		public UnionMemberController(
			ApplicationDbContext context,
			ILoggerFactory loggerFactory,
			IHostingEnvironment env)
		{
			db = context;
			_logger = loggerFactory.CreateLogger<UnionMemberController>();
			_env = env;
		}

		//
		// GET: /Users/
		[HttpGet]
		public async Task<IActionResult> Index()
		{
			return View(await db.UnionMembers.AsNoTracking().ToListAsync());
		}

		//
		// GET: /Users/Create
		[HttpGet]
		public async Task<IActionResult> Create()
		{
			var model = new UnionMemberViewModel();
			return View(model);
		}

		//
		// POST: /Users/Create
		[HttpPost]
		public async Task<IActionResult> Create(UnionMemberViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				var model = new UnionMember(viewModel);
				var webRoot = _env.WebRootPath;
				if (viewModel.Picture != null)
				{
					var file = await Code.SaveFile(viewModel.Picture, webRoot, viewModel.Name);
					if (!string.IsNullOrWhiteSpace(file)) // Spara endast om en bild har blivit uppladdad.
						model.PictureSrc = file;
				}
				await db.UnionMembers.AddAsync(model);
				await db.SaveChangesAsync();
				TempData["Message"] = "Medlem skapad!";
				return RedirectToAction("Index");
			}
			return View();
		}

		//
		// GET: /Users/Edit/1
		[HttpGet]
		public async Task<IActionResult> Edit(int id)
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
		public async Task<IActionResult> Edit(UnionMemberViewModel viewModel)
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
				model.Phone = viewModel.Phone;
				model.Quote = viewModel.Quote;

				var webRoot = _env.WebRootPath;
				if (viewModel.Picture != null)
				{
					var file = await Code.SaveFile(viewModel.Picture, webRoot, viewModel.Name);
					if (!string.IsNullOrWhiteSpace(file)) // Spara endast om en bild har blivit uppladdad.
						model.PictureSrc = file;
				}

				await db.SaveChangesAsync();
				TempData["Message"] = "Medlem uppdaterad!";
				return RedirectToAction("Index");
			}
			return View();
		}
		//
		// GET: /Users/Delete/
		[HttpGet]
		public async Task<IActionResult> Delete(int id)
		{
			var user = await db.UnionMembers.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);
			if (user == null)
				return RedirectToAction("Index");

			return View(user);
		}

		//
		// POST: /Users/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			if (ModelState.IsValid)
			{
				var user = await db.UnionMembers.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);
				if (user == null)
					return RedirectToAction("Index");

				db.UnionMembers.Remove(user);
				await db.SaveChangesAsync();
				TempData["Message"] = "Medlem raderad!";
				return RedirectToAction("Index");
			}
			return View();
		}
	}
}

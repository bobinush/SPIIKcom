using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SPIIKcom.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using SPIIKcom.Models;
using SPIIKcom.ViewModels;
using SPIIKcom.Services;
using System;
using SPIIKcom.Filters;

namespace SPIIKcom.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Route("Admin")]
	[Authorize(Roles = "Admin, Styrelse")]
	public class HomeController : Controller
	{
		private readonly ApplicationDbContext _db;
		private readonly SpiikService _spiikService;

		public HomeController(ApplicationDbContext context, SpiikService spiikService)
		{
			_db = context;
			_spiikService = spiikService;
		}
		[HttpGet]
		public async Task<IActionResult> Index()
		{
			var model = await _db.Organization.AsNoTracking().SingleOrDefaultAsync();
			var viewModel = new ForeningViewModel(model);
			viewModel.SlideshowSrc = _spiikService.GetImages("slideshow");
			return View(viewModel);
		}
		[HttpPost]
		[RequestFormSizeLimit(2097152)] // Max image size 2 MB
		public async Task<IActionResult> Index(ForeningViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				if (viewModel.Logo?.Length > 0)
					await _spiikService.SaveFile(viewModel.Logo, "images", "logo");

				if (viewModel.Slideshow?.Count > 0)
					await _spiikService.SaveFiles(viewModel.Slideshow, "images/slideshow", "slide");

				var model = await _db.Organization.SingleOrDefaultAsync();
				model.Name = viewModel.Name;
				model.Abbreviation = viewModel.Abbreviation;
				model.Address = viewModel.Address;
				model.PostalCode = viewModel.PostalCode;
				model.City = viewModel.City;
				model.Email = viewModel.Email;
				model.Phone = viewModel.Phone;
				model.GoogleLink = viewModel.GoogleLink;
				model.TwitterAPIKey = viewModel.TwitterAPIKey;
				model.GoogleAPIKey = viewModel.GoogleAPIKey;
				model.FacebookAPIId = viewModel.FacebookAPIId;
				model.FacebookAPIKey = viewModel.FacebookAPIKey;
				model.Instagram = viewModel.Instagram;
				// _db.Entry(model).State = EntityState.Modified;

				await _db.SaveChangesAsync();
				TempData["Message"] = "Uppgifter uppdaterade!";
				return RedirectToAction("Index");
			}
			return View("Index", viewModel);
		}
	}
}

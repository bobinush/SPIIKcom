using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using SPIIKcom.Data;
using SPIIKcom.Filters;
using SPIIKcom.Services;
using SPIIKcom.ViewModels;

namespace SPIIKcom.Controllers
{
	public class HomeController : Controller
	{
		private readonly ApplicationDbContext _db;
		private readonly SpiikService _spiikService;
		public HomeController(ApplicationDbContext context, SpiikService spiikservice)
		{
			_db = context;
			_spiikService = spiikservice;
		}
		public async Task<IActionResult> Index()
		{
			// TODO : Get Instagram posts.
			List<string> model = _spiikService.GetImages("slideshow");
			return View(model);
		}

		public async Task<IActionResult> Kontakt()
		{
			return View(await _db.Organization.AsNoTracking().SingleOrDefaultAsync());
		}

		public IActionResult Error()
		{
			var statusCode = TempData["Error"].ToString();
			statusCode += " " + ((HttpStatusCode)int.Parse(statusCode)).ToString(); ;
			return View("Error", statusCode);
		}

		[HttpPost]
		//[AjaxOnly]
		public IActionResult SendContactForm(ContactQuestionViewModel viewModel)
		{

			string msg = "what what";
			if (ModelState.IsValid)
			{
				bool success = false;
				try
				{
					// Send email
					success = true;
				}
				catch (Exception ex)
				{
					// log ex.Message?
					msg = "Error while sending.";
				}
			}
			return Json(msg);
		}
		[HttpPost]
		public IActionResult FbWebhook()
		{
			/*
			Instead of getting new posts, why not let facebook send us the new posts?
			https://developers.facebook.com/docs/graph-api/webhooks/
			*/
			return null;
		}
	}
}

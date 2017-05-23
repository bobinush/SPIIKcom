using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using SPIIKcom.Data;
using SPIIKcom.Filters;
using SPIIKcom.ViewModels;

namespace SPIIKcom.Controllers
{
	public class HomeController : Controller
	{
		private readonly ApplicationDbContext db;
		public HomeController(ApplicationDbContext context)
		{
			db = context;
		}
		public async Task<IActionResult> Index()
		{
			// TODO : Get Instagram posts.
			return View();
		}

		public async Task<IActionResult> Kontakt()
		{
			return View(await db.Organization.AsNoTracking().FirstOrDefaultAsync());
		}

		public IActionResult Error()
		{
			return View();
		}

		[HttpPost]
		//[AjaxOnly]
		public IActionResult SendContactForm(ContactQuestionViewModel viewModel)
		{
			string msg = "what what";
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

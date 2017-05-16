using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SPIIKcom.Filters;
using SPIIKcom.ViewModels;

namespace SPIIKcom.Controllers
{
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Kontakt()
		{
			ViewData["Message"] = "Your contact page.";
			return View();
		}

		public IActionResult Error()
		{
			return View();
		}

		[HttpPost]
		//[AjaxOnly]
		public IActionResult SendContactForm(QuestionViewModel viewModel)
		{
			string msg = "what what";
			bool success = false;
			try
			{
				// long size = 0;
				// var files = Request.Form.Files;
				// foreach (var file in files)
				// {
				// 	var filename = System.Net.Http.Headers.ContentDispositionHeaderValue
				// 					.Parse(file.ContentDisposition)
				// 					.FileName
				// 					.Trim('"');
				// 	filename = _env.WebRootPath + $@"\{filename}";
				// 	size += file.Length;
				// 	using (System.IO.FileStream fs = System.IO.File.Create(filename))
				// 	{
				// 		file.CopyTo(fs);
				// 		fs.Flush();
				// 	}
				// }
				// msg = $"{files.Count} file(s) / {size} bytes uploaded successfully!";
				success = true;
			}
			catch (Exception ex)
			{
				msg = ex.Message;
			}

			return Json(msg);
		}
	}
}

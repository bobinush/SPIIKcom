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
	public class FileController : Controller
	{
		private readonly ApplicationDbContext db;
		private readonly ILogger _logger;
		private IHostingEnvironment _env;

		public FileController(
			ApplicationDbContext context,
			ILoggerFactory loggerFactory,
			IHostingEnvironment env)
		{
			db = context;
			_logger = loggerFactory.CreateLogger<UnionMemberController>();
			_env = env;
		}

		[HttpPost]
		public IActionResult UploadAjax()
		{
			string msg;
			bool success = false;
			try
			{
				long size = 0;
				var files = Request.Form.Files;
				foreach (var file in files)
				{
					var filename = System.Net.Http.Headers.ContentDispositionHeaderValue
									.Parse(file.ContentDisposition)
									.FileName
									.Trim('"');
					filename = _env.WebRootPath + $@"\{filename}";
					size += file.Length;
					using (System.IO.FileStream fs = System.IO.File.Create(filename))
					{
						file.CopyTo(fs);
						fs.Flush();
					}
				}
				msg = $"{files.Count} file(s) / {size} bytes uploaded successfully!";
				success = true;
			}
			catch (Exception ex)
			{
				msg = ex.Message;
			}

			return Json(new { success = success, msg = msg });
		}
	}
}


/*
Kul att göra i framtiden
https://code.msdn.microsoft.com/How-to-achieve-upload-cdfe216c

<label asp-for="Picture" class="btn btn btn-default"><i class="fa fa-upload fa-lg"></i>&nbsp; Ladda upp en profilbild</label>
<input asp-for="Picture" type="file" style="display: none;">
<i id="fa-loading" class="fa fa-spinner fa-spin" style="display: none;"></i>
<i id="fa-checkmark" class="fa fa-check" aria-hidden="true" style="display: none;"></i>
<i id="fa-error" class="fa fa-exclamation-triangle" aria-hidden="true" style="display: none;"></i>

<script>
	document.getElementById("Picture").onchange = function() { // input type="file"
		$("#fa-loading").show();
		$("#fa-error").hide();
		$("#fa-checkmark").hide();

		var data = new FormData();
		data.append($("#Name").val(), $(this).get(0).files[0]);
		$.ajax({
			type: "POST",
			url: "/admin/file/UploadAjax",
			contentType: false,
			processData: false,
			data: data,
			success: function (data) {
				$("#fa-loading").hide();
				if(data.success)
				{
					$("#fa-checkmark").show();
				}
				else{
					alert(data.msg);
					$("#fa-error").show();
				}
			},
			error: function (xhr, ajaxOptions, thrownError) {
				$("#fa-loading").hide();
				$("#fa-error").show();
				alert(xhr.status + "\n" + thrownError);
				alert(xhr.responseJSON.Message);
			}
		});
	};
	</script>
 */
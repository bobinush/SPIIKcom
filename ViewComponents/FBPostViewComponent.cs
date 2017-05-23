using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SPIIKcom.Models;
using SPIIKcom.ViewModels;
using SPIIKcom.Data;
using Microsoft.Extensions.Options;

namespace SPIIKcom.ViewComponents
{
	public class FBPostViewComponent : ViewComponent
	{

		private readonly ApplicationDbContext db;
		public AppKeyConfig AppConfig { get; }
		public FBPostViewComponent(ApplicationDbContext context, IOptions<AppKeyConfig> appkeys)
		{
			AppConfig = appkeys.Value;
			db = context;
		}

		public async Task<IViewComponentResult> InvokeAsync(int numberOfPosts = 20)
		{
			List<FacebookPost> model = await Code.GetFacebookPosts(AppConfig, numberOfPosts);
			return View(model);
		}
	}
}
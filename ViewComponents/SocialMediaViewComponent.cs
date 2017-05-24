using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SPIIKcom.Models;
using SPIIKcom.ViewModels;
using SPIIKcom.Data;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Hosting;

namespace SPIIKcom.ViewComponents
{
	public class SocialMediaViewComponent : ViewComponent
	{
		private readonly ApplicationDbContext db;
		private IHostingEnvironment env;
		public AppKeyConfig AppConfig { get; }
		private readonly SpiikService _spiikService;
		public SocialMediaViewComponent(ApplicationDbContext context,
			IOptions<AppKeyConfig> appkeys,
			IHostingEnvironment environment,
			SpiikService spiikService)
		{
			AppConfig = appkeys.Value;
			db = context;
			env = environment;
			_spiikService = spiikService;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			string PageId, AccessToken;
			if (env.IsDevelopment())
			{
				PageId = AppConfig.FacebookAPIId;
				AccessToken = AppConfig.FacebookAPIKey;
			}
			else
			{
				var organization = await db.Organization.FirstOrDefaultAsync();
				PageId = organization.FacebookAPIId;
				AccessToken = organization.FacebookAPIKey;
			}
			List<FacebookPost> fb = await _spiikService.GetFacebookPosts(PageId, AccessToken);
			List<Item> ig = await _spiikService.GetInstagramPosts();
			var igList = ig.Select(x => new SocialMedia
			{
				PermalinkUrl = x.PermalinkUrl,
				Username = x.User.Username,
				CreatedTime = x.CreatedTime,
				Picture = x.Picture,
				Text = x.Text,
				Type = "Instagram"
			});
			var fbList = fb.Select(x => new SocialMedia
			{
				PermalinkUrl = x.PermalinkUrl,
				Username = x.User.Username,
				CreatedTime = x.CreatedTime,
				Picture = x.Picture,
				Text = x.Text,
				Type = "Facebook"
			});
			var list = new List<SocialMedia>();
			list.AddRange(igList);
			list.AddRange(fbList);
			return View(list.OrderByDescending(x => x.CreatedTime).ToList());
		}
	}
}
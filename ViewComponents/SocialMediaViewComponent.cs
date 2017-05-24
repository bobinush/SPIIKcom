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
using SPIIKcom.Services;

namespace SPIIKcom.ViewComponents
{
	public class SocialMediaViewComponent : ViewComponent
	{
		private readonly ApplicationDbContext _db;
		private IHostingEnvironment _env;
		public readonly AppKeyConfig _appConfig;
		private readonly SpiikService _spiikService;
		public SocialMediaViewComponent(ApplicationDbContext context,
			IOptions<AppKeyConfig> appkeys,
			IHostingEnvironment environment,
			SpiikService spiikService)
		{
			_appConfig = appkeys.Value;
			_db = context;
			_env = environment;
			_spiikService = spiikService;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			string fbPageId, fbAccessToken, instagramUrl;
			var list = new List<SocialMedia>();

			if (_env.IsDevelopment())
			{
				fbPageId = _appConfig.FacebookAPIId;
				fbAccessToken = _appConfig.FacebookAPIKey;
				instagramUrl = _appConfig.Instagram;
			}
			else
			{
				var organization = await _db.Organization.AsNoTracking().SingleOrDefaultAsync();
				instagramUrl = organization.Instagram;
				fbPageId = organization.FacebookAPIId;
				fbAccessToken = organization.FacebookAPIKey;
			}

			// Get Facebook posts
			if (!string.IsNullOrWhiteSpace(fbPageId) && !string.IsNullOrWhiteSpace(fbAccessToken))
			{
				List<FacebookPost> fb = await _spiikService.GetFacebookPosts(fbPageId, fbAccessToken);
				var fbList = fb.Select(x => new SocialMedia
				{
					PermalinkUrl = x.PermalinkUrl,
					Username = x.User.Username,
					CreatedTime = x.CreatedTime,
					Picture = x.Picture,
					Text = x.Text,
					Type = "Facebook"
				});
				list.AddRange(fbList);
			}

			// Get Instagram posts
			if (!string.IsNullOrWhiteSpace(instagramUrl))
			{
				List<Item> ig = await _spiikService.GetInstagramPosts(instagramUrl);
				var igList = ig.Select(x => new SocialMedia
				{
					PermalinkUrl = x.PermalinkUrl,
					Username = x.User.Username,
					CreatedTime = x.CreatedTime,
					Picture = x.Picture,
					Text = x.Text,
					Type = "Instagram"
				});
				list.AddRange(igList);
			}

			return View(list.OrderByDescending(x => x.CreatedTime).ToList());
		}
	}
}
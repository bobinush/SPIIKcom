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
	public class SocialMediaViewComponent : ViewComponent
	{
		private readonly ApplicationDbContext db;
		public AppKeyConfig AppConfig { get; }
		public SocialMediaViewComponent(ApplicationDbContext context, IOptions<AppKeyConfig> appkeys)
		{
			AppConfig = appkeys.Value;
			db = context;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			List<FacebookPost> fb = await Code.GetFacebookPosts(AppConfig);
			List<Item> ig = await Code.GetInstagramPosts();
			var igList = ig.Select(x => new SocialMedia {
				PermalinkUrl = x.PermalinkUrl,
				Username = x.User.Username,
				CreatedTime = x.CreatedTime,
				Picture = x.Picture,
				Text = x.Text,
				Type = "Instagram"});
			var fbList = fb.Select(x => new SocialMedia {
				PermalinkUrl = x.PermalinkUrl,
				Username = x.User.Username,
				CreatedTime = x.CreatedTime,
				Picture = x.Picture,
				Text = x.Text,
				Type = "Facebook"});
			var list = new List<SocialMedia>();
			list.AddRange(igList);
			list.AddRange(fbList);
			return View(list.OrderByDescending(x => x.CreatedTime).ToList());
		}
	}
}
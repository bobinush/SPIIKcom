using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SPIIKcom.Data;
using SPIIKcom.Enums;
using SPIIKcom.Extensions;
using SPIIKcom.Models;
using SPIIKcom.ViewModels;

namespace SPIIKcom
{
	public class Code
	{
		private readonly ApplicationDbContext db;
		private IHostingEnvironment _environment;
		public Code(ApplicationDbContext context, IHostingEnvironment environment)
		{
			db = context;
			_environment = environment;
		}
		/// <summary>
		///  Get the Enum as a list with SelectListItems
		/// </summary>
		/// <returns></returns>
		public static List<SelectListItem> GetUnionTypeselectList()
		{
			return Enum.GetValues(typeof(UnionTypeEnum))
				.Cast<UnionTypeEnum>()
				// .Where(x => x != UnionMemberEnum.None)
				.Select(x => new SelectListItem()
				{
					Text = x.GetDisplayName(),
					Value = ((int)x).ToString()
				}).ToList();
		}

		/// <summary>
		/// Sparar en fil
		/// </summary>
		/// <param name="file">Filen som skall sparas</param>
		/// <param name="path">wwwroot</param>
		/// <param name="fileName">Valfri: Överskrid filnamnet (utan filändelse)</param>
		/// <returns>Filnamnet inkl. filändelse</returns>
		internal static async Task<string> SaveFile(IFormFile file, string path, string folder, string fileName = null)
		{
			// full path to file in temp location
			string name = "";
			if (file.Length > 0)
			{
				var filePath = Path.Combine(path, folder, fileName ?? file.FileName);
				if (!filePath.EndsWith(Path.GetExtension(file.FileName)))
					filePath += Path.GetExtension(file.FileName);

				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					await file.CopyToAsync(stream);
				}
				name = Path.GetFileName(filePath);
			}
			return name; // Returnerar endast filnamnet om en fil har blivit uppladdad.
		}

		/// <summary>
		/// Gets Facebook posts from the specified Facebook page.
		/// </summary>
		/// <param name="numberOfPosts">Number of total posts to get. (default/min 20)</param>
		/// <returns></returns>
		internal static async Task<List<FacebookPost>> GetFacebookPosts(AppKeyConfig AppConfig, int numberOfPosts = 20)
		{
			var fbList = new List<FacebookPost>();
			FacebookViewModel viewModel = new FacebookViewModel();
			using (var client = new HttpClient())
			{
				try
				{
					int numberofFeeds = 20;
					string PageId = AppConfig.FacebookAPIId;
					string AccessToken = AppConfig.FacebookAPIKey;

					string FeedRequestUrl = string.Concat("https://graph.facebook.com/" + PageId + "/posts?limit=",
						numberofFeeds,
						"&access_token=",
						AccessToken,
						@"&fields=
						full_picture,
						picture,
						link,
						message,
						created_time,
						description,
						id,
						caption,
						is_published,
						name,
						object_id,
						type,
						is_hidden,
						permalink_url,
						from"
					);
					while (!string.IsNullOrWhiteSpace(FeedRequestUrl) && fbList.Count < numberOfPosts )
					{

						var response = await client.GetAsync(FeedRequestUrl);
						response.EnsureSuccessStatusCode(); // Throw in not success

						var stringResponse = await response.Content.ReadAsStringAsync();
						viewModel = JsonConvert.DeserializeObject<FacebookViewModel>(stringResponse);
						FeedRequestUrl = viewModel.Paging.Next;
						fbList.AddRange(viewModel.Data);
					}
				}
				catch (HttpRequestException e)
				{
					Console.WriteLine($"Request exception: {e.Message}");
				}
			}
			return fbList;
		}
	}
}

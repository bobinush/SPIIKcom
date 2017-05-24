using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SPIIKcom.Data;
using SPIIKcom.Enums;
using SPIIKcom.Extensions;
using SPIIKcom.Models;
using SPIIKcom.ViewModels;

namespace SPIIKcom.Services
{
	public class SpiikService
	{
		private ApplicationDbContext _db;
		private IHostingEnvironment _env;
		public readonly AppKeyConfig _appConfig;

		public SpiikService(ApplicationDbContext context,
			IHostingEnvironment environment,
			IOptions<AppKeyConfig> appkeys)
		{
			_db = context;
			_env = environment;
		}

		/// <summary>
		/// Saves a file
		/// </summary>
		/// <param name="file">File to be saved</param>
		/// <param name="path">wwwroot</param>
		/// <param name="folder">A folder inside wwwroot</param>
		/// <param name="fileName">Optional: New filename (without extension). The new filename will be in Kebab Case.</param>
		/// <returns>Filename with extension</returns>
		internal async Task<string> SaveFile(IFormFile file, string path, string folder, string fileName = null)
		{
			string name = "";
			if (file.Length > 0)
			{
				string fileNameKebabCase = (fileName ?? file.FileName).ToLower().Replace(" ", "-");
				string filePath = Path.Combine(path, folder, fileNameKebabCase);
				if (!filePath.EndsWith(Path.GetExtension(file.FileName)))
					filePath += Path.GetExtension(file.FileName);

				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					await file.CopyToAsync(stream);
				}
				name = Path.GetFileName(filePath);
			}
			return name;
		}

		/// <summary>
		/// Gets Facebook posts from the specified Facebook page.
		/// </summary>
		/// <param name="pageId">The Facebook page id. (name in url)</param>
		/// <param name="accessToken">Permanent access token</param>
		/// <returns></returns>
		internal async Task<List<FacebookPost>> GetFacebookPosts(string pageId, string accessToken)
		{
			// https://developers.facebook.com/docs/graph-api
			// TODO : Webhooks https://developers.facebook.com/docs/graph-api/webhooks
			var fbList = new List<FacebookPost>();
			var viewModel = new FacebookViewModel();
			using (var client = new HttpClient())
			{
				try
				{
					int numberOfFeeds = 20, // Number of feeds to get per request
						numberOfPosts = 20; // Number of total posts (numberOfFeeds * n)

					// To add or remove fields below: Add/Remove them to the ViewModel as well.
					string FeedRequestUrl = string.Concat("https://graph.facebook.com/" + pageId + "/posts?limit=",
						numberOfFeeds,
						"&access_token=",
						accessToken,
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
					while (!string.IsNullOrWhiteSpace(FeedRequestUrl) && fbList.Count < numberOfPosts)
					{

						var response = await client.GetAsync(FeedRequestUrl);
						response.EnsureSuccessStatusCode(); // Throw in not success

						var stringResponse = await response.Content.ReadAsStringAsync();
						viewModel = JsonConvert.DeserializeObject<FacebookViewModel>(stringResponse);
						FeedRequestUrl = viewModel.Paging.Next;
						fbList.AddRange(viewModel.Posts);
					}
				}
				catch (HttpRequestException e)
				{
					Console.WriteLine($"Request exception: {e.Message}");
				}
			}
			return fbList;
		}

		/// <summary>
		/// Gets Instagram posts from the specified Instagram page.
		/// </summary>
		/// <param name="url">Url to the Instagram page</param>
		/// <returns></returns>
		internal async Task<List<Item>> GetInstagramPosts(string url)
		{
			var instagramList = new List<Item>();
			var viewModel = new InstagramViewModel();
			using (var client = new HttpClient())
			{
				try
				{
					// Check for last char.
					url = (url.Right(1) == "/" ? url.Substring(0, url.Length - 1) : url);
					string FeedRequestUrl = string.Concat(url + "/media/");
					var response = await client.GetAsync(FeedRequestUrl);
					response.EnsureSuccessStatusCode(); // Throw in not success

					var stringResponse = await response.Content.ReadAsStringAsync();
					viewModel = JsonConvert.DeserializeObject<InstagramViewModel>(stringResponse);
					instagramList.AddRange(viewModel.items);
				}
				catch (HttpRequestException e)
				{
					Console.WriteLine($"Request exception: {e.Message}");
				}
			}
			return instagramList;
		}
	}
}

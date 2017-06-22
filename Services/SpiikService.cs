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

		internal List<string> GetImages(string folder)
		{
			var bildList = new List<string>();
			folder = Path.Combine("images", folder + "/").Replace("\\", "/");
			string path = Path.Combine(_env.WebRootPath, folder);
			if (Directory.Exists(path))
			{
				//bildList = Directory.GetFiles(path, "*.*", SearchOption.TopDirectoryOnly)
				//					.Select(Path.GetFileName).ToList();
				var allowedExts = new List<string> { ".jpg", ".jpeg", ".gif", ".png", ".bmp" };
				DirectoryInfo dir = new DirectoryInfo(Path.GetDirectoryName(path));
				FileInfo[] subFiles = dir.GetFiles().Where(s => allowedExts.Contains(Path.GetExtension(s.Extension))).ToArray();

				bildList.AddRange(subFiles.Select(x => "/" + folder + x.Name));
			}
			return bildList;
		}

		/// <summary>
		/// Saves multiple files with the fileName + index
		/// </summary>
		/// <param name="files">File to be saved</param>
		/// <param name="folder">A folder inside wwwroot</param>
		/// <param name="fileName">Optional: New filename (without extension). The new filename will be in Kebab Case.</param>
		/// <returns>saved: true/false and filename with extension or errormessage</returns>
		internal async Task SaveFiles(List<IFormFile> files, string folder, string fileName)
		{
			for (int i = 0; i < files.Count; i++)
			{
				await SaveFile(files[i], folder, fileName + i);
			}
		}
		/// <summary>
		/// Saves a file
		/// </summary>
		/// <param name="file">File to be saved</param>
		/// <param name="folder">A folder inside wwwroot</param>
		/// <param name="fileName">Optional: New filename (without extension). The new filename will be in Kebab Case.</param>
		/// <returns>saved: true/false and relative path+filename with extension or errormessage</returns>
		internal async Task<Tuple<bool, string>> SaveFile(IFormFile file, string folder, string fileName = null)
		{
			string msg = "";
			bool success = false;
			if (file.Length > 5000000) // 5 MB limit
				msg = "The picture size cannot exceed 5MB.";
			else
			{
				string fileNameKebabCase = HtmlExtensions.URLFriendly(fileName ?? file.FileName);
				string filePath = Path.Combine(_env.WebRootPath, folder);
				string filePathAndName = Path.Combine(filePath, fileNameKebabCase);
				if (!filePathAndName.EndsWith(Path.GetExtension(file.FileName)))
					filePathAndName += Path.GetExtension(file.FileName);

				Directory.CreateDirectory(filePath);
				using (var stream = new FileStream(filePathAndName, FileMode.Create))
				{
					await file.CopyToAsync(stream);
				}
				msg = Path.Combine(folder, Path.GetFileName(filePathAndName));
				success = true;
			}
			return new Tuple<bool, string>(success, msg);
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
		internal async Task<List<Item>> GetInstagramPosts(string instagramId)
		{
			var instagramList = new List<Item>();
			var viewModel = new InstagramViewModel();
			using (var client = new HttpClient())
			{
				try
				{
					string FeedRequestUrl = string.Concat("https://www.instagram.com/" + instagramId + "/media/");
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

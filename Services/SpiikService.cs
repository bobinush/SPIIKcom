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
		private ApplicationDbContext db;
		private IHostingEnvironment env;
		public SpiikService(ApplicationDbContext context, IHostingEnvironment environment)
		{
			db = context;
			env = environment;
		}

		/// <summary>
		/// Sparar en fil
		/// </summary>
		/// <param name="file">Filen som skall sparas</param>
		/// <param name="path">wwwroot</param>
		/// <param name="fileName">Valfri: Överskrid filnamnet (utan filändelse). Det nya filnamnet kommer bli i Kebab Case.</param>
		/// <returns>Filnamnet inkl. filändelse</returns>
		internal async Task<string> SaveFile(IFormFile file, string path, string folder, string fileName = null)
		{
			// full path to file in temp location
			string name = "";
			if (file.Length > 0)
			{
				string lowerFileName = (fileName ?? file.FileName).ToLower().Replace(" ", "-");
				string filePath = Path.Combine(path, folder, lowerFileName);
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
					int numberofFeeds = 20,
						numberOfPosts = 20;
					string FeedRequestUrl = string.Concat("https://graph.facebook.com/" + pageId + "/posts?limit=",
						numberofFeeds,
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
		/// <returns></returns>
		internal async Task<List<Item>> GetInstagramPosts()
		{
			var instagramList = new List<Item>();
			var viewModel = new IGVM();
			using (var client = new HttpClient())
			{
				try
				{
					string FeedRequestUrl = string.Concat("https://www.instagram.com/spiikalmar" + "/media/");
					var response = await client.GetAsync(FeedRequestUrl);
					response.EnsureSuccessStatusCode(); // Throw in not success

					var stringResponse = await response.Content.ReadAsStringAsync();
					viewModel = JsonConvert.DeserializeObject<IGVM>(stringResponse);
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

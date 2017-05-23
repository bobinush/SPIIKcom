using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace SPIIKcom.ViewModels
{
	public class User
	{
		public string id { get; set; }
		public string full_name { get; set; }
		public string profile_picture { get; set; }
		public string Username { get; set; }
	}

	public class Image
	{
		public int width { get; set; }
		public int height { get; set; }
		public string url { get; set; }
	}

	public class Images
	{
		public Image thumbnail { get; set; }
		public Image low_resolution { get; set; }
		public Image standard_resolution { get; set; }
	}

	public class From
	{
		public string id { get; set; }
		public string full_name { get; set; }
		public string profile_picture { get; set; }
		public string username { get; set; }
	}

	public class Caption
	{
		public string id { get; set; }
		public string text { get; set; }
		public string created_time { get; set; }
		public From from { get; set; }
	}

	public class Datum
	{
		public string id { get; set; }
		public string full_name { get; set; }
		public string profile_picture { get; set; }
		public string username { get; set; }
	}

	public class Likes
	{
		public List<Datum> data { get; set; }
		public int count { get; set; }
	}

	public class Comments
	{
		public List<object> data { get; set; }
		public int count { get; set; }
	}

	public class Location
	{
		public string name { get; set; }
	}

	public class Video
	{
		public int width { get; set; }
		public int height { get; set; }
		public string url { get; set; }
	}

	public class Videos
	{
		public Video standard_resolution { get; set; }
		public Video low_bandwidth { get; set; }
		public Video low_resolution { get; set; }
	}

	public class Item
	{
		public string id { get; set; }
		public string code { get; set; }
		public User User { get; set; }
		public Images images { get; set; }
		public string created_time { get; set; }
		public Caption caption { get; set; }
		public bool user_has_liked { get; set; }
		public Likes likes { get; set; }
		public Comments comments { get; set; }
		public bool can_view_comments { get; set; }
		public bool can_delete_comments { get; set; }
		public string type { get; set; }
		public string link { get; set; }
		public Location location { get; set; }
		public string alt_media_url { get; set; }
		public Videos videos { get; set; }
		public int? video_views { get; set; }


		public string Picture { get { return images.thumbnail.url; } }
		public string Text { get { return caption?.text; } }
		public string PermalinkUrl { get { return link; } }
		public DateTime CreatedTime
		{
			get
			{
				DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
				return dtDateTime.AddSeconds(Convert.ToDouble(created_time)).ToLocalTime();
			}
		}
	}

	public class IGVM
	{
		public List<Item> items { get; set; }
		public bool more_available { get; set; }
		public string status { get; set; }
	}


	public class SocialMedia
	{
		public string PermalinkUrl { get; set; }
		public string Username { get; set; }
		public DateTime CreatedTime { get; set; }
		public string Picture { get; set; }
		public string Text { get; set; }
		public string Type { get; set; }
	}
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace SPIIKcom.ViewModels
{
	[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
	public class InstagramViewModel
	{
		[JsonProperty(PropertyName = "items")]
		public IEnumerable<InstagramPost> Posts { get; set; }
		[JsonProperty(PropertyName = "more_available")]
		public bool MoreAvailable { get; set; }
		[JsonProperty(PropertyName = "status")]
		public string Status { get; set; }
	}

	[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
	public class InstagramPost
	{
		[JsonProperty(PropertyName = "id")]
		public string Id { get; set; }
		[JsonProperty(PropertyName = "code")]
		public string Code { get; set; }
		[JsonProperty(PropertyName = "user")]
		public InstagramUser User { get; set; }
		[JsonProperty(PropertyName = "images")]
		public List<InstagramImages> Images { get; set; }
		public bool IsHidden { get; set; }
		[JsonProperty(PropertyName = "created_time")]
		public DateTime CreatedTime { get; set; }
		[JsonProperty(PropertyName = "caption")]
		public InstagramCaption Caption { get; set; }
		[JsonProperty(PropertyName = "likes")]
		public InstagramLikes Likes { get; set; }
		[JsonProperty(PropertyName = "type")]
		public string Type { get; set; }
		[JsonProperty(PropertyName = "link")]
		public string Link { get; set; }
	}

	[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
	public class InstagramUser
	{
		[JsonProperty(PropertyName = "id")]
		public string Id { get; set; }
		[JsonProperty(PropertyName = "full_name")]
		public string Name { get; set; }
		[JsonProperty(PropertyName = "profile_picture")]
		public string ProfilePicture { get; set; }
		[JsonProperty(PropertyName = "username")]
		public string Username { get; set; }
	}


	[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
	public class InstagramImages
	{
		[JsonProperty(PropertyName = "thumbnail")]
		public InstagramImage Thumbnail { get; set; }
		[JsonProperty(PropertyName = "low_resolution")]
		public InstagramImage LowResolution { get; set; }
		[JsonProperty(PropertyName = "standard_resolution")]
		public InstagramImage StandardResolution { get; set; }
	}
	[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
	public class InstagramImage
	{
		[JsonProperty(PropertyName = "width")]
		public int Width { get; set; }
		[JsonProperty(PropertyName = "height")]
		public int Height { get; set; }
		[JsonProperty(PropertyName = "url")]
		public string Url { get; set; }
	}
	[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
	public class InstagramCaption
	{
		[JsonProperty(PropertyName = "id")]
		public string Id { get; set; }
		[JsonProperty(PropertyName = "test")]
		public string Test { get; set; }
		[JsonProperty(PropertyName = "created_time")]
		public DateTime CreatedTime { get; set; }
	}
	[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
	public class InstagramLikes
	{
		[JsonProperty(PropertyName = "data")]
		public List<InstagramUser> Users { get; set; }
		[JsonProperty(PropertyName = "count")]
		public int Count { get; set; }
	}
}

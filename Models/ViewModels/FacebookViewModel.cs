using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace SPIIKcom.ViewModels
{
	[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
	public class FacebookViewModel
	{
		[JsonProperty(PropertyName = "data")]
		public IEnumerable<FacebookPost> Posts { get; set; }
		[JsonProperty(PropertyName = "paging")]
		public FacebookPaging Paging { get; set; }
	}

	[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
	public class FacebookPaging
	{
		[JsonProperty(PropertyName = "previous")]
		public string Previous { get; set; }
		[JsonProperty(PropertyName = "next")]
		public string Next { get; set; }

	}

	[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
	public class FacebookPost
	{
		[JsonProperty(PropertyName = "id")]
		public string Id { get; set; }
		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }
		[JsonProperty(PropertyName = "is_published")]
		public bool IsPublished { get; set; }
		[JsonProperty(PropertyName = "is_hidden")]
		public bool IsHidden { get; set; }
		[JsonProperty(PropertyName = "full_picture")]
		public string FullPicture { get; set; }
		[JsonProperty(PropertyName = "picture")]
		public string Picture { get; set; }
		[JsonProperty(PropertyName = "link")]
		public string Link { get; set; }
		[JsonProperty(PropertyName = "message")]
		public string Message { get; set; }
		[JsonProperty(PropertyName = "description")]
		public string Description { get; set; }
		[JsonProperty(PropertyName = "caption")]
		public string Caption { get; set; }
		[JsonProperty(PropertyName = "object_id")]
		public string ObjectId { get; set; }
		[JsonProperty(PropertyName = "type")]
		public string Type { get; set; }
		[JsonProperty(PropertyName = "created_time")]
		public DateTime CreatedTime { get; set; }
		[JsonProperty(PropertyName = "permalink_url")]
		public string PermalinkUrl { get; set; }
		[JsonProperty(PropertyName = "from")]
		public FacebookPage User { get; set; }


		public string Text { get { return Message ?? Description; } }

	}

	[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
	public class FacebookPage
	{
		[JsonProperty(PropertyName = "id")]
		public string Id { get; set; }
		[JsonProperty(PropertyName = "name")]
		public string Username { get; set; }
	}
}

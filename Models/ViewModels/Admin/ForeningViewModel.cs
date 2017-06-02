using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using SPIIKcom.Models;

namespace SPIIKcom.ViewModels
{
	public class ForeningViewModel
	{
		public ForeningViewModel()
		{

		}
		public ForeningViewModel(Organization model)
		{
			// Id = model.Id;
			Name = model.Name;
			Abbreviation = model.Abbreviation;
			Address = model.Address;
			PostalCode = model.PostalCode;
			City = model.City;
			Email = model.Email;
			Phone = model.Phone;
			GoogleLink = model.GoogleLink;
			TwitterAPIKey = model.TwitterAPIKey;
			GoogleAPIKey = model.GoogleAPIKey;
			FacebookAPIId = model.FacebookAPIId;
			FacebookAPIKey = model.FacebookAPIKey;
			Instagram = model.Instagram;
		}
		public int Id { get; set; }
		[Display(Name = "Namn")]
		public string Name { get; set; }
		[Display(Name = "Förkortning")]
		public string Abbreviation { get; set; }
		[Display(Name = "Adress")]
		public string Address { get; set; }
		[Display(Name = "Postkod")]
		[DataType(DataType.PostalCode)]
		public string PostalCode { get; set; }
		[Display(Name = "Ort")]
		public string City { get; set; }
		[Display(Name = "E-post")]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }
		[Display(Name = "Telefon")]
		[DataType(DataType.PhoneNumber)]
		public string Phone { get; set; }
		[Display(Name = "Google Maps länk")]
		[DataType(DataType.Url)]
		public string GoogleLink { get; set; }
		public string TwitterAPIKey { get; set; }
		public string GoogleAPIKey { get; set; }
		[Display(Name = "Facebook")]
		public string FacebookAPIId { get; set; }
		[Display(Name = "Facebook access token")]
		public string FacebookAPIKey { get; set; }
		public string Instagram { get; set; }
		[DataType(DataType.Upload)]
		[Display(Name = "100x51 Logga.png")]
		public IFormFile Logo { get; set; }
		[DataType(DataType.Upload)]
		[Display(Name = "Bildspel")]
		public List<IFormFile> Slideshow { get; set; }
		public List<string> SlideshowSrc { get; set; }
	}
}

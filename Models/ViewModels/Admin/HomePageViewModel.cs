using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SPIIKcom.ViewModels
{
	public class HomePageViewModel
	{
		[Display(Name = "Namn")]
		public string Name { get; set; }
		[Display(Name="Förkortning")]
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
		[Display(Name="Google Maps länk")]
		[DataType(DataType.Url)]
		public string GoogleLink { get; set; }
	}
}

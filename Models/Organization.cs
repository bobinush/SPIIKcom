using System;
using System.ComponentModel.DataAnnotations;
using SPIIKcom.ViewModels;

namespace SPIIKcom.Models
{
	public class Organization
	{
		public int Id { get; set; }
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
		public string TwitterAPIKey { get; set; }
		public string GoogleAPIKey { get; set; }
		[Display(Name="Facebook")]
		public string FacebookAPIId { get; set; }
		[Display(Name="Facebook access token")]
		public string FacebookAPIKey { get; set; }
		public string Instagram { get; set; }

		// TODO: Lägg till url-länkar till sociala medier

	}
}

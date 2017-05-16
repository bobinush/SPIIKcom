using System.ComponentModel.DataAnnotations;

namespace SPIIKcom.ViewModels
{
	public class ContactPageViewModel
	{
		public string Address { get; set; }
		public string PostalCode { get; set; }
		public string City { get; set; }
		public string OpeningHours { get; set; }
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }
		[DataType(DataType.PhoneNumber)]
		public string Phone { get; set; }
		public string Message { get; set; }
	}
}

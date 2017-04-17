using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SPIIKcom.ViewModels
{
	// Add profile data for application users by adding properties to the ApplicationUser class
	public class CreateMemberViewModel
	{
		[Required]
		public string PersonalNumber { get; set; }
		[Required]
		public string Name { get; set; }
		[Required]
		public string LastName { get; set; }
		public string Address { get; set; }
		[DataType(DataType.PostalCode)]
		public string PostalCode { get; set; }
		public string City { get; set; }
		[Required]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }
		[DataType(DataType.PhoneNumber)]
		public string Phone { get; set; }

		[Range(0, 10, ErrorMessage = "Du måste välja en typ av Membership")]
		public uint MembershipTypeId { get; set; }
		public SelectList MembershipTypes { get; set; }

	}
}

using System;
using System.ComponentModel.DataAnnotations;
using SPIIKcom.ViewModels;

namespace SPIIKcom.Models
{
	public class Member
	{
		public Member()
		{
		}
		public Member(CreateMemberViewModel vm)
		{
			PersonalNumber = vm.PersonalNumber;
			FirstName = vm.Name;
			LastName = vm.LastName;
			Address = vm.Address;
			PostalCode = vm.PostalCode;
			City = vm.City;
			Email = vm.Email;
			Phone = vm.Phone;
			JoinDate = DateTime.Today;
		}

		public int Id { get; set; }
		[Required]
		public string PersonalNumber { get; set; }
		[Required]
		public string FirstName { get; set; }
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
		[DataType(DataType.ImageUrl)]
		public string Picture { get; set; }
		public string Program { get; set; }
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		public DateTime JoinDate { get; set; }
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		public DateTime ExpireDate { get; set; }


		public string FullName { get { return FirstName + " " + LastName; } }
	}
}

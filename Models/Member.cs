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
			FirstName = vm.FirstName;
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
		[Display(Name = "Förnamn")]
		public string FirstName { get; set; }
		[Required]
		[Display(Name = "Efternamn")]
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
		public string Program { get; set; }
		[Display(Name = "Medlem fr.o.m")]
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
		public DateTime JoinDate { get; set; }
		[Display(Name = "Medlem t.o.m")]
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
		public DateTime ExpireDate { get; set; }

		public string FullName { get { return FirstName + " " + LastName; } }
	}
}

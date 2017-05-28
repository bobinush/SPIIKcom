using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using SPIIKcom.Models;

namespace SPIIKcom.ViewModels
{
	// Add profile data for application users by adding properties to the IdentityUser class
	public class CreateMemberViewModel
	{
		public CreateMemberViewModel()
		{
		}
		public CreateMemberViewModel(Member model)
		{
			FirstName = model.FirstName;
			LastName = model.LastName;
			Address = model.Address;
			PostalCode = model.PostalCode;
			City = model.City;
			Email = model.Email;
			Phone = model.Phone;
			Program = model.Program;
		}
		[Required]
		[Display(Name = "Personnummer")]
		[RegularExpression(@"^[0-9]+$", ErrorMessage = "Endast siffror tillåtna i personnummer")] // This accepts one or more digits.
		public string PersonalNumber { get; set; }
		[Required]
		[Display(Name = "Förnamn")]
		public string FirstName { get; set; }
		[Required]
		[Display(Name = "Efternamn")]
		public string LastName { get; set; }
		[Display(Name = "Adress")]
		public string Address { get; set; }
		[Display(Name = "Postnummer")]
		[DataType(DataType.PostalCode)]
		public string PostalCode { get; set; }
		[Display(Name = "Ort")]
		public string City { get; set; }
		[Required]
		[Display(Name = "E-post")]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }
		[Display(Name = "Telefon")]
		[DataType(DataType.PhoneNumber)]
		public string Phone { get; set; }
		[Display(Name = "Program")]
		public string Program { get; set; }
		[Required]
		[Display(Name = "Medlemskap")]
		[Range(0, 10, ErrorMessage = "Du måste välja en typ av medlemskap")]
		public int MembershipTypeId { get; set; }
		public SelectList MembershipTypes { get; set; }
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		public DateTime JoinDate { get; }
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		public DateTime ExpireDate { get; }
	}
	public class EditMemberViewModel
	{
		public EditMemberViewModel()
		{
		}
		public EditMemberViewModel(Member model)
		{
			Id = model.Id;
			FirstName = model.FirstName;
			LastName = model.LastName;
			PersonalNumber = model.PersonalNumber;
			Address = model.Address;
			PostalCode = model.PostalCode;
			City = model.City;
			Email = model.Email;
			Phone = model.Phone;
			Program = model.Program;
			ExpireDate = model.ExpireDate;
			JoinDate = model.JoinDate;
		}
		public int Id { get; set; }
		[Required]
		[Display(Name = "Personnummer")]
		[RegularExpression(@"^[0-9]+$", ErrorMessage = "Endast siffror tillåtna i personnummer")] // This accepts one or more digits.
		public string PersonalNumber { get; set; }
		[Required]
		[Display(Name = "Förnamn")]
		public string FirstName { get; set; }
		[Required]
		[Display(Name = "Efternamn")]
		public string LastName { get; set; }
		[Display(Name = "Adress")]
		public string Address { get; set; }
		[Display(Name = "Postnummer")]
		[DataType(DataType.PostalCode)]
		public string PostalCode { get; set; }
		[Display(Name = "Ort")]
		public string City { get; set; }
		[Required]
		[Display(Name = "E-post")]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }
		[Display(Name = "Telefon")]
		[DataType(DataType.PhoneNumber)]
		public string Phone { get; set; }
		[Display(Name = "Program")]
		public string Program { get; set; }
		[Display(Name = "Nytt medlemskap")]
		public int MembershipTypeId { get; set; }
		public SelectList MembershipTypes { get; set; }
		[Display(Name = "Medlem fr.o.m")]
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
		public DateTime JoinDate { get; }
		[Display(Name = "Medlem t.o.m")]
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
		public DateTime ExpireDate { get; }
		public string ExpireStatus { get; set; }
	}
}

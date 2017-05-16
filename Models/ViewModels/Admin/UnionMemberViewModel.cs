using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using SPIIKcom.Enums;
using SPIIKcom.Models;

namespace SPIIKcom.ViewModels
{
	// Add profile data for application users by adding properties to the IdentityUser class
	public class UnionMemberViewModel
	{
		public UnionMemberViewModel()
		{
		}
		public UnionMemberViewModel(UnionMember model)
		{
			Id = model.Id;
			Name = model.Name;
			Title = model.Title;
			Email = model.Email;
			Phone = model.Phone;
			Quote = model.Quote;
			PictureSrc = model.PictureSrc;
			UnionTypes = model.UnionTypes;
		}

		public int Id { get; set; }
		[Required]
		[Display(Name = "Namn")]
		public string Name { get; set; }
		[Required]
		[Display(Name = "Titel")]
		public string Title { get; set; }
		[Required]
		[DataType(DataType.EmailAddress)]
		[Display(Name = "E-post")]
		public string Email { get; set; }
		[DataType(DataType.PhoneNumber)]
		[Display(Name="Telefonnummer")]
		public string Phone { get; set; }
		[Display(Name="Citat")]
		public string Quote { get; set; }
		[DataType(DataType.Upload)]
		[Display(Name="Profilbild")]
		public IFormFile Picture { get; set; }
		[DataType(DataType.ImageUrl)]
		public string PictureSrc { get; set; }
		[Display(Name="Roller")]
		public UnionTypeEnum UnionTypes { get; set; }
	}
}

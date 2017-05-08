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
			Quote = model.Quote;
			PictureSrc = model.PictureSrc;
			UnionTypes = model.UnionTypes;
		}

		public int Id { get; set; }
		[Required]
		public string Name { get; set; }
		[Required]
		public string Title { get; set; }
		[Required]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }
		[DataType(DataType.PhoneNumber)]
		public string Quote { get; set; }
		[DataType(DataType.Upload)]
		public IFormFile Picture { get; set; }
		public string PictureSrc { get; set; }
		public UnionTypeEnum UnionTypes { get; set; }
		public UnionTypeEnum[] SelectedRoles { get; set; }
	}
}

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using SPIIKcom.Enums;

namespace SPIIKcom.Models.AccountViewModels
{
	public class RegisterUserViewModel
	{
		[Required]
		[EmailAddress]
		[Display(Name = "Email")]
		public string Email { get; set; }
		[Required]
		[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string Password { get; set; }
		[DataType(DataType.Password)]
		[Display(Name = "Confirm password")]
		[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; }
		public string Name { get; set; }
		public IEnumerable<SelectListItem> RolesList { get; set; }
		[Display(Name = "Användaroller")]
		public string[] SelectedRoles { get; set; }
	}
}

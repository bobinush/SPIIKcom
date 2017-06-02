using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using SPIIKcom.Enums;

namespace SPIIKcom.Models.AccountViewModels
{
	public class EditUserViewModel
	{
		public string Id { get; set; }
		[Required(AllowEmptyStrings = false)]
		[Display(Name = "Email")]
		[EmailAddress]
		public string Email { get; set; }
		public string Name { get; set; }
		[Required]
		[Display(Name = "Användaroller")]
		public string[] SelectedRoles { get; set; }
		public IEnumerable<SelectListItem> RolesList { get; set; }
		// public UserTypeEnum SelectedRoles { get; set; }
	}
}

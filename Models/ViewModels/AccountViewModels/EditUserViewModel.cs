using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SPIIKcom.Models.AccountViewModels
{
	public class EditUserViewModel
	{
		public string Id { get; set; }

		[Required]
		[Display(Name = "Email")]
		[EmailAddress]
		public string Email { get; set; }

		public IEnumerable<SelectListItem> RolesList { get; set; }
	}
}
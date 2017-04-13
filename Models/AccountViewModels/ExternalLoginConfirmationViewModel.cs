using System.ComponentModel.DataAnnotations;

namespace SPIIKcom.Models.AccountViewModels
{
	public class ExternalLoginConfirmationViewModel
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; }
	}
}

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SPIIKcom.ViewModels
{
	public class ContactQuestionViewModel
	{
		public string Name { get; set; }
		[Required]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }
		[Required]
		public string Message { get; set; }
	}
}

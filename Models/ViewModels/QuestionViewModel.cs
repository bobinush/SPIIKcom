using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SPIIKcom.ViewModels
{
	public class QuestionViewModel
	{
		[Required]
		public string Name { get; set; }
		[Required]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }
		[DataType(DataType.PhoneNumber)]
		public string Phone { get; set; }
		public string Message { get; set; }
	}
}

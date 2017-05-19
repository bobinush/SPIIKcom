using System.ComponentModel.DataAnnotations;

namespace SPIIKcom.Models
{
	public class Stadga
	{
		public int Id { get; set; }
		[Required]
		public string Number { get; set; }
		[Required]
		public string Name { get; set; }
		[Required]
		public string Text { get; set; }
	}
}
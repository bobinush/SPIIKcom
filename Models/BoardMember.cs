using System.ComponentModel.DataAnnotations;

namespace SPIIKcom.Models
{
	public class BoardMember
	{
		public int ID { get; set; }
		[Required]
		public string Name { get; set; }
		[Required]
		public string Role { get; set; }
		[Required]
		[DataType(DataType.EmailAddress)]
		public string BoardEmail { get; set; }
		public string Quote { get; set; }
	}
}

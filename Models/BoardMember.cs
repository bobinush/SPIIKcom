using System.ComponentModel.DataAnnotations;
using SPIIKcom.Enums;

namespace SPIIKcom.Models
{
	public class BoardMember
	{
		public int ID { get; set; }
		[Required]
		public string Name { get; set; }
		[Required]
		public string Role { get; set; }
		public BoardType BoardType { get; set; }
		[Required]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }
		public string Quote { get; set; }
		public string Picture { get; set; }

	}
}

using System.ComponentModel.DataAnnotations;

namespace SPIIKcom.Models
{
	public class MembershipType
	{
		public int Id { get; set; }
		[Required]
		public string Name { get; set; }
		[Required]
		public decimal Price { get; set; }
		[Required]
		public int LengthInYears { get; set; }
	}
}

using System.ComponentModel.DataAnnotations;

namespace SPIIKcom.Models
{
	public class MembershipType
	{
		public int Id { get; set; }
		[Required]
		[Display(Name = "Namn")]
		public string Name { get; set; }
		[Required]
		[Display(Name = "Pris")]
		public decimal Price { get; set; }
		[Required]
		[Display(Name = "Längd (år)")]
		public int LengthInYears { get; set; }
	}
}

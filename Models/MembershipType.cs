using System.ComponentModel.DataAnnotations;

namespace SPIIKcom.Models
{
	// Add profile data for application users by adding properties to the ApplicationUser class
	public class MembershipType
	{
		public int ID { get; set; }
		[Required]
		public string Name { get; set; }
		[Required]
		public decimal Price { get; set; }
		[Required]		
		public double LengthInYears { get; set; }
	}
}

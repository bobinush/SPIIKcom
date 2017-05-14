using System.ComponentModel.DataAnnotations;
using SPIIKcom.Enums;
using SPIIKcom.ViewModels;

namespace SPIIKcom.Models
{
	public class UnionMember
	{
		public UnionMember() {
		}
		public UnionMember(UnionMemberViewModel vm) {
			Id = vm.Id;
			Name = vm.Name;
			Title = vm.Title;
			UnionTypes = vm.UnionTypes;
			Email = vm.Email;
			Phone = vm.Phone;
			Quote = vm.Quote;
			PictureSrc = vm.PictureSrc;
		}
		public int Id { get; set; }
		[Required]
		public string Name { get; set; }
		[Required]
		public string Title { get; set; }
		public UnionTypeEnum UnionTypes { get; set; }
		[Required]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }
		public string Phone { get; set; }
		public string Quote { get; set; }
		public string PictureSrc { get; set; }
	}
}

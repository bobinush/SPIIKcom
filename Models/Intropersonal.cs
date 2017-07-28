using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Http;
using SPIIKcom.Enums;
using SPIIKcom.ViewModels;

namespace SPIIKcom.Models
{
	public class IntroPersonal
	{
		public IntroPersonal()
		{
		}
		public IntroPersonal(IntroPersonalViewModel viewModel)
		{
			Name = viewModel.Name;
			NickName = viewModel.NickName;
			Age = viewModel.Age;
			Program = viewModel.Program;
			Quote = viewModel.Quote;
			Bribe = viewModel.Bribe;
			GoodWord = viewModel.GoodWord;
			PictureSrc = viewModel.PictureSrc;
			IntroType = viewModel.IntroType;
		}

		public int Id { get; set; }
		[Display(Name = "Namn")]
		public string Name { get; set; }
		[Display(Name = "Smeknamn")]
		public string NickName { get; set; }
		[Display(Name = "Ålder")]
		public string Age { get; set; }
		[Display(Name = "Pluggar")]
		public string Program { get; set; }
		[Display(Name = "Citat")]
		public string Quote { get; set; }
		[Display(Name = "Favoritmuta")]
		public string Bribe { get; set; }
		[Display(Name = "Staben säger")]
		public string GoodWord { get; set; }
		[DataType(DataType.ImageUrl)]
		public string PictureSrc { get; set; }
		public IntroTypeEnum IntroType { get; set; }

		public string FullName
		{
			get
			{
				string name = Name;
				if (!string.IsNullOrWhiteSpace(NickName))
				{
					var nameSplit = Name.Split(' ');
					// Adds the nickname as: firstname "nickname" lastname
					name = nameSplit[0] + " \"" + NickName + "\" " + string.Join(" ", nameSplit.Skip(1).ToArray());
				}
				return name;
			}
		}
	}
}

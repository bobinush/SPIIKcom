using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using SPIIKcom.Enums;
using SPIIKcom.Models;

namespace SPIIKcom.ViewModels
{
	public class IntroViewModel
	{
		// public IntroViewModel()
		// {
		// }

		public List<IntroPersonal> Personal { get; set; }
		public List<StaticPage> Text { get; set; }
	}
	public class IntroPersonalViewModel
	{
		public IntroPersonalViewModel()
		{
		}
		public IntroPersonalViewModel(IntroPersonal model)
		{
			Id = model.Id;
			Name = model.Name;
			NickName = model.NickName;
			Age = model.Age;
			Program = model.Program;
			Quote = model.Quote;
			Bribe = model.Bribe;
			GoodWord = model.GoodWord;
			PictureSrc = model.PictureSrc;
			IntroType = model.IntroType;
		}

		public int Id { get; set; }
		[Display(Name = "Namn")]
		public string Name { get; set; }
		[Display(Name = "Smeknamn")]
		public string NickName { get; set; }
		public string Age { get; set; }
		[Display(Name = "Pluggar")]
		public string Program { get; set; }
		[Display(Name = "Citat")]
		public string Quote { get; set; }
		[Display(Name = "Favoritmuta")]
		public string Bribe { get; set; }
		[Display(Name = "Staben säger")]
		public string GoodWord { get; set; }
		[DataType(DataType.Upload)]
		[Display(Name="Profilbild")]
		public IFormFile Picture { get; set; }
		[DataType(DataType.ImageUrl)]
		public string PictureSrc { get; set; }
		[Display(Name="Roll")]
		public IntroTypeEnum IntroType { get; set; }
		public string FullName
		{
			get
			{
				var name = Name.Split(' ');
				return name[0] + "\"" + NickName + "\"" + string.Join(" ", name, 1, name.Length - 1);
			}
		}
	}
}

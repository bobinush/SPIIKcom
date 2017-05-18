using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using SPIIKcom.Models;

namespace SPIIKcom.ViewModels
{
	public class StadgaViewModel
	{
		public List<Stadga> Stadgar { get; set; }
	}
}

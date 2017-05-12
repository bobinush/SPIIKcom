using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using SPIIKcom.Data;
using SPIIKcom.Enums;
using SPIIKcom.Extensions;

namespace SPIIKcom
{
	public class Code
	{
		private readonly ApplicationDbContext db;
		private IHostingEnvironment _environment;
		public Code(ApplicationDbContext context, IHostingEnvironment environment)
		{
			db = context;
			_environment = environment;
		}
		/// <summary>
		///  Get the Enum as a list with SelectListItems
		/// </summary>
		/// <returns></returns>
		public static List<SelectListItem> GetUnionTypeselectList()
		{
			return Enum.GetValues(typeof(UnionTypeEnum))
				.Cast<UnionTypeEnum>()
				// .Where(x => x != UnionMemberEnum.None)
				.Select(x => new SelectListItem()
				{
					Text = x.GetDisplayName(),
					Value = ((int)x).ToString()
				}).ToList();
		}

		/// <summary>
		/// Sparar en fil
		/// </summary>
		/// <param name="file">Filen som skall sparas</param>
		/// <param name="path">wwwroot</param>
		/// <param name="fileName">Valfri: Överskrid filnamnet (utan filändelse)</param>
		/// <returns>Filnamnet inkl. filändelse</returns>
		internal static async Task<string> SaveFile(IFormFile file, string path, string fileName = null)
		{
			// full path to file in temp location
			string name = "";
			if (file.Length > 0)
			{
				var filePath = Path.Combine(path, fileName ?? file.FileName);
				if (!filePath.EndsWith(Path.GetExtension(file.FileName)))
					filePath += Path.GetExtension(file.FileName);

				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					await file.CopyToAsync(stream);
				}
				name = Path.GetFileName(filePath);
			}
			return name; // Returnerar endast filnamnet om en fil har blivit uppladdad.
		}
	}
}

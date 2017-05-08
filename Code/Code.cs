using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using SPIIKcom.Data;
using SPIIKcom.Enums;
using SPIIKcom.Extensions;

namespace SPIIKcom
{
	public class Code
	{
		private readonly ApplicationDbContext db;
		public Code(ApplicationDbContext context)
		{
			db = context;
		}
		/// <summary>
		///  Get the Enum as a list with SelectListItems
		/// </summary>
		/// <returns></returns>
		public static List<SelectListItem> GetUnionTypeselectList() {
			return Enum.GetValues(typeof(UnionTypeEnum))
				.Cast<UnionTypeEnum>()
				// .Where(x => x != UnionMemberEnum.None)
				.Select(x => new SelectListItem()
				{
					Text = x.GetDisplayName(),
					Value = ((int)x).ToString()
				}).ToList();
		}
	}
}

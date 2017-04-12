using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SPIIKcom
{
	public class RouteConfig
	{
		public static void ConfigureRoute(IRouteBuilder routes)
		{
			//Home/Index
			routes.MapRoute(
				name: "default",
				template: "{controller=Home}/{action=Index}/{id?}"
			);
		}
	}
}

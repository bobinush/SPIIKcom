using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace SPIIKcom.Filters
{
	public class AjaxOnlyAttribute : ActionMethodSelectorAttribute
	{
		// http://rion.io/2015/08/11/detecting-ajax-requests-in-asp-net-mvc6/
		public override bool IsValidForRequest(RouteContext routeContext, ActionDescriptor action)
		{
			return (string)routeContext.HttpContext.Request?.Headers["X-Requested-With"] == "XMLHttpRequest";
		}
	}
}
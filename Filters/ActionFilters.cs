using System;
using Microsoft.AspNetCore.Http.Features;
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

	/// <summary>
	/// Filter to set size limits for request form data
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
	public class RequestFormSizeLimitAttribute : Attribute, IAuthorizationFilter, IOrderedFilter
	{
		private readonly FormOptions _formOptions;

		public RequestFormSizeLimitAttribute(int valueCountLimit)
		{
			_formOptions = new FormOptions()
			{
				ValueCountLimit = valueCountLimit
			};
		}

		public int Order { get; set; }

		public void OnAuthorization(AuthorizationFilterContext context)
		{
			var features = context.HttpContext.Features;
			var formFeature = features.Get<IFormFeature>();

			if (formFeature == null || formFeature.Form == null)
			{
				// Request form has not been read yet, so set the limits
				features.Set<IFormFeature>(new FormFeature(context.HttpContext.Request, _formOptions));
			}
		}
}
}
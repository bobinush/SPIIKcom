using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SPIIKcom.Extensions
{
	public static class HtmlExtensions
	{
		private static readonly HtmlContentBuilder _emptyBuilder = new HtmlContentBuilder();

		public static IHtmlContent BuildBreadcrumbNavigation(this IHtmlHelper helper)
		{
			if (helper.ViewContext.RouteData.Values["controller"].ToString() == "Home" ||
				helper.ViewContext.RouteData.Values["controller"].ToString() == "Account")
			{
				return _emptyBuilder;
			}

			string controllerName = helper.ViewContext.RouteData.Values["controller"].ToString();
			string actionName = helper.ViewContext.RouteData.Values["action"].ToString();

			var breadcrumb = new HtmlContentBuilder()
								.AppendHtml("<ol class='breadcrumb'><li>")
								.AppendHtml(helper.ActionLink("Home", "Index", "Home"))
								.AppendHtml("</li><li>")
								.AppendHtml(helper.ActionLink(controllerName.FirstLetterToUpperCase(), "Index", controllerName))
								.AppendHtml("</li>");



			if (helper.ViewContext.RouteData.Values["action"].ToString() != "Index")
			{
				breadcrumb.AppendHtml("<li>")
						  .AppendHtml(helper.ActionLink(actionName.FirstLetterToUpperCase(), actionName, controllerName))
						  .AppendHtml("</li>");
			}

			return breadcrumb.AppendHtml("</ol>");
		}
	}
}
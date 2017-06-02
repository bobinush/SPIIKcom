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

		#region URL-Friendly
		/// <summary>
		/// Produces optional, URL-friendly version of a title, "like-this-one".
		/// hand-tuned for speed, reflects performance refactoring contributed
		/// by John Gietzen (user otac0n)
		/// </summary>
		// http://stackoverflow.com/questions/25259/how-does-stack-overflow-generate-its-seo-friendly-urls/25486#25486
		public static string URLFriendly(string title)
		{
			if (title == null) return string.Empty;

			const int maxlen = 80;
			int len = title.Length;
			bool prevdash = false;
			var sb = new System.Text.StringBuilder(len);
			char c;

			for (int i = 0; i < len; i++)
			{
				c = title[i];
				if ((c >= 'a' && c <= 'z') || (c >= '0' && c <= '9'))
				{
					sb.Append(c);
					prevdash = false;
				}
				else if (c >= 'A' && c <= 'Z')
				{
					// tricky way to convert to lowercase
					sb.Append((char)(c | 32));
					prevdash = false;
				}
				else if (c == ' ' || c == ',' || c == '.' || c == '/' ||
					c == '\\' || c == '-' || c == '_' || c == '=')
				{
					if (!prevdash && sb.Length > 0)
					{
						sb.Append('-');
						prevdash = true;
					}
				}
				else if ((int)c >= 128)
				{
					int prevlen = sb.Length;
					sb.Append(RemapInternationalCharToAscii(c));
					if (prevlen != sb.Length) prevdash = false;
				}
				if (i == maxlen) break;
			}

			if (prevdash)
				return sb.ToString().Substring(0, sb.Length - 1);
			else
				return sb.ToString();
		}

		// http://meta.stackexchange.com/questions/7435/non-us-ascii-characters-dropped-from-full-profile-url/7696#7696
		public static string RemapInternationalCharToAscii(char c)
		{
			string s = c.ToString().ToLowerInvariant();
			if ("àåáâäãåą".Contains(s))
			{
				return "a";
			}
			else if ("èéêëę".Contains(s))
			{
				return "e";
			}
			else if ("ìíîïı".Contains(s))
			{
				return "i";
			}
			else if ("òóôõöøőð".Contains(s))
			{
				return "o";
			}
			else if ("ùúûüŭů".Contains(s))
			{
				return "u";
			}
			else if ("çćčĉ".Contains(s))
			{
				return "c";
			}
			else if ("żźž".Contains(s))
			{
				return "z";
			}
			else if ("śşšŝ".Contains(s))
			{
				return "s";
			}
			else if ("ñń".Contains(s))
			{
				return "n";
			}
			else if ("ýÿ".Contains(s))
			{
				return "y";
			}
			else if ("ğĝ".Contains(s))
			{
				return "g";
			}
			else if (c == 'ř')
			{
				return "r";
			}
			else if (c == 'ł')
			{
				return "l";
			}
			else if (c == 'đ')
			{
				return "d";
			}
			else if (c == 'ß')
			{
				return "ss";
			}
			else if (c == 'Þ')
			{
				return "th";
			}
			else if (c == 'ĥ')
			{
				return "h";
			}
			else if (c == 'ĵ')
			{
				return "j";
			}
			else
			{
				return "";
			}
		}
		#endregion
	}
}
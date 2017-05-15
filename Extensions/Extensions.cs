using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace SPIIKcom.Extensions
{
	public static class Extensions
	{
		/// <summary>
		/// How to get the Display Name Attribute of an Enum member?
		/// http://stackoverflow.com/a/26455406/1695663
		/// </summary>
		public static string GetDisplayName(this Enum enumValue)
		{
			return enumValue.GetType()
							.GetMember(enumValue.ToString())
							.First()
							.GetCustomAttribute<DisplayAttribute>()
							.GetName();
		}
		/// <summary>
		/// Returns the input string with the first character converted to uppercase, or mutates any nulls passed into string.Empty
		/// </summary>
		public static string FirstLetterToUpperCase(this string s)
		{
			if (string.IsNullOrEmpty(s))
				return string.Empty;

			char[] a = s.ToCharArray();
			a[0] = char.ToUpper(a[0]);
			return new string(a);
		}
	}
}
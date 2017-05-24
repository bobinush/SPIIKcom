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

		/// <summary>
		/// Retunerar antalet bokstäver från vänster
		/// </summary>
		/// <param name="str"></param>
		/// <param name="length">Antalet bokstäver som ska returneras</param>
		/// <returns></returns>
		public static string Left(this string str, int length)
		{
			if (string.IsNullOrEmpty(str))
				return str;
			return (str.Length <= length ? str : str.Substring(0, length));
		}

		/// <summary>
		/// Retunerar antalet bokstöver från höger
		/// </summary>
		/// <param name="str"></param>
		/// <param name="length">Antalet bokstäver som ska returneras</param>
		/// <returns></returns>
		public static string Right(this string str, int length)
		{
			if (string.IsNullOrEmpty(str))
				return str;
			return (str.Length <= length ? str : str.Substring(str.Length - length));
		}

		/// <summary>
		/// Kollar om värdet är mellan 2 värden.
		/// </summary>
		/// <param name="eLowValue">Lägsta värdet</param>
		/// <param name="eHighValue">Högsta värdet</param>
		public static bool Between(this IComparable value, IComparable eLowValue, IComparable eHighValue)
		{
			return value.CompareTo(eLowValue) >= 0 && value.CompareTo(eHighValue) <= 0;
		}
	}
}
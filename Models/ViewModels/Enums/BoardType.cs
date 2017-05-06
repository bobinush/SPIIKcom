using System;

namespace SPIIKcom.Enums
{
	// Enum, Bitwise och Flags for dummies: http://stackoverflow.com/a/8480/1695663
	// Advanced: https://www.codeproject.com/Articles/544990/Understand-how-bitwise-operators-work-Csharp-and-V
	[Flags]
	public enum BoardType
	{
		None = 0,
		Styrelse = 1,
		Sexmasteri = 2,
		Revisor = 4,
		Valberedningen = 8
	}
}
// public static MvcHtmlString VisaUtrymme(this HtmlHelper helper, int utrymme = 0)
// {
// 	string output = string.Empty;
// 	if (utrymme > 0)
// 	{
// 		// 158 är en kombination av 128 + 16 + 8 + 4 + 2 + 0 (=158)
// 		foreach (Utrymme u in Enum.GetValues(typeof(Utrymme)))
// 		{
// 			if (u == Utrymme.EjAngivet)
// 				continue; // skip the current iteration.
// 			if ((utrymme & (int)u) == (int)u)
// 			{
// 				output += u.GetDisplayName() + ", ";
// 			}
// 		}
// 	}
// 	// Finns inget utrymme, visa ej angivet. UTF-8 &nsbp; "\u00A0"
// 	output = string.IsNullOrWhiteSpace(output.Left(output.Length - 2)) ? Utrymme.EjAngivet.GetDisplayName() : output.Left(output.Length - 2);
// 	return MvcHtmlString.Create(output);
// }
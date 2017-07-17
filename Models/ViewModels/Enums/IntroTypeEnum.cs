using System;
using System.ComponentModel.DataAnnotations;

namespace SPIIKcom.Enums
{
	// Enum, Bitwise och Flags for dummies: http://stackoverflow.com/a/8480/1695663
	// Advanced: https://www.codeproject.com/Articles/544990/Understand-how-bitwise-operators-work-Csharp-and-V
	[Flags]
	public enum IntroTypeEnum
	{
		// None = 0,
		[Display(Name = "STAB")]
		Stab = 1,
		[Display(Name = "Fadder")]
		Fadder = 2,
		// 4, 8, 16, 32 osv..
	}
}
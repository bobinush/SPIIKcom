using System;
using System.ComponentModel.DataAnnotations;

namespace SPIIKcom.Enums
{
	// Enum, Bitwise och Flags for dummies: http://stackoverflow.com/a/8480/1695663
	// Advanced: https://www.codeproject.com/Articles/544990/Understand-how-bitwise-operators-work-Csharp-and-V
	[Flags]
	public enum UserTypeEnum
	{
		Admin = 1,
		Styrelse = 2,
	}
}
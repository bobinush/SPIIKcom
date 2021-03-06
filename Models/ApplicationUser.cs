﻿using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace SPIIKcom.Models
{
	// Add profile data for application users by adding properties to the ApplicationUser class
	public class ApplicationUser : IdentityUser
	{
		public string Name { get; set; }
		public DateTime SignupDate { get; set; }
	}
}

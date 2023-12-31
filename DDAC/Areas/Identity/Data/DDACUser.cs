﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace DDAC.Areas.Identity.Data;

// Add profile data for application users by adding properties to the DDACUser class
public class DDACUser : IdentityUser
{
    [PersonalData]
    public string userFullName { get; set; }
	[PersonalData]
	public string userRole { get; set; }
}


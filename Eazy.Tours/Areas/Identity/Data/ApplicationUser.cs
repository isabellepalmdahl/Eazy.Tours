using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Eazy.Tours.Areas.Identity.Data;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    //public string Id { get; set; }

    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    public override string Email { get; set; }
    public override string PhoneNumber { get; set; }

    

}
public class ApplicationRole : IdentityRole
{

}


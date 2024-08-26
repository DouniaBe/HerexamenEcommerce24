using HerexamenEcommerce24.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

public class ManageUsersViewModel
{
    public IList<ApplicationUser> Users { get; set; }
    public IList<IdentityRole> Roles { get; set; }
}

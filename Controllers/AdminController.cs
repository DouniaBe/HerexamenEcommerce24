using HerexamenEcommerce24.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AdminController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public IActionResult ManageUsers()
    {
        // Verkrijg gebruikers en rollen
        var users = _userManager.Users.ToList();
        var roles = _roleManager.Roles.ToList();
        // Retourneer een view
        return View(new ManageUsersViewModel
        {
            Users = users,
            Roles = roles
        });
    }

    public async Task<IActionResult> ChangeUserRole(string userId, string roleName)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound();
        }

        await _userManager.AddToRoleAsync(user, roleName);
        return RedirectToAction(nameof(ManageUsers));
    }
}

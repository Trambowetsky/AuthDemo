using AuthDemo.Data;
using AuthDemo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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

    public async Task<IActionResult> Index()
    {
        List<UserViewModel> model = new List<UserViewModel>();
        var users = _userManager.Users.ToList();
        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            model.Add(new UserViewModel()
            {
                Email = user.Email,
                Id = user.Id,
                Roles = roles.ToList()
            });
        }

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> AssignRole(string userId, string role)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null && await _roleManager.RoleExistsAsync(role))
        {
            await _userManager.AddToRoleAsync(user, role);
        }

        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> EditRoles(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        var model = new EditRolesViewModel
        {
            UserId = user.Id,
            Email = user.Email,
            AllRoles = _roleManager.Roles.Select(r => r.Name).ToList(),
            UserRoles = (await _userManager.GetRolesAsync(user)).ToList()
        };

        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> EditRoles(string id, List<string> selectedRoles)
    {
        var user = await _userManager.FindByIdAsync(id);
        var currentUserRoles = (await _userManager.GetRolesAsync(user)).ToList();
        var removeRoles = currentUserRoles.Except(selectedRoles);
        var addRoles =selectedRoles.Except(currentUserRoles);
        
        await _userManager.RemoveFromRolesAsync(user, removeRoles);
        await _userManager.AddToRolesAsync(user, addRoles);
        return RedirectToAction("Index");
    }
}
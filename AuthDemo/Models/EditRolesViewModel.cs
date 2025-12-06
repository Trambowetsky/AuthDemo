namespace AuthDemo.Models;

public class EditRolesViewModel
{
    public string UserId { get; set; }
    public string Email { get; set; }

    public List<string> AllRoles { get; set; } = new();
    public List<string> UserRoles { get; set; } = new();
}
using Microsoft.AspNetCore.Identity;

namespace AuthDemo.Data;

public class ApplicationUser : IdentityUser
{
    public string? FullName { get; set; }
}
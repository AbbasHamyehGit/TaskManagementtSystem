using Microsoft.AspNetCore.Identity;

namespace TaskManagementAPI.Models
{
    // Extend IdentityUser or IdentityUser<TKey> as needed
    public class ApplicationUser : IdentityUser<int>
    {
        // Additional properties
        public string? FullName { get; set; }
        public bool IsAdmin { get; set; }
    }
}

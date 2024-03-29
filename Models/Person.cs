using Microsoft.AspNetCore.Identity;

namespace TaskManagementAPI.Models
{ 
    public class Person : IdentityUser<int>

{ 
        public string Password { get; set; }
        public bool IsAdmin { get; set; }

        // Constructor to initialize non-nullable properties
        public Person()
        {
            Password = "passwordSTRONG123"; // Provide a default value or use another mechanism to set the password
        }
    }
}

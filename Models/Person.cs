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
            Password = "RWEGFRS@$THGTYEJRETSY46";
        }
    }
}

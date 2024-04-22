using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.Models;
using Task = TaskManagementAPI.Models.Task;
using Person = TaskManagementAPI.Models.Person;
namespace TaskManagementAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Person> Person { get; set; } 
        public DbSet<Task> Tasks { get; set; } 
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    }
}

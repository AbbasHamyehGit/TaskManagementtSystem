namespace TaskManagementAPI.Models
{
    public class Task
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool? IsAssigned { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsCritical { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime DueDate { get; set; }

        // Foreign key
        public int PersonId { get; set; }
    }
}
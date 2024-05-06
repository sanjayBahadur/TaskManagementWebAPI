namespace TaskManagement.Models
{
    public class Task
    {
        public int ID { get; set; }
        public string? Name { get; set; }

        public string? Description { get; set; }

        public bool HighPriority { get; set; }

        public bool IsCompleted { get; set; }

    }
}

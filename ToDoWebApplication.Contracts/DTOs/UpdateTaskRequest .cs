namespace ToDoWebApplication.Contracts.DTOs
{
    public class UpdateTaskRequest
    {
        public string? Description { get; set; } = string.Empty;
        public bool? IsCompleted { get; set; }
    }
}

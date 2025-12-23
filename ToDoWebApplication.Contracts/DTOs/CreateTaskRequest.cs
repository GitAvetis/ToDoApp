using System.ComponentModel.DataAnnotations;

namespace ToDoWebApplication.Contracts.DTOs
{
    public class CreateTaskRequest
    {
        [Required(ErrorMessage = "Task description is required")]
        [MinLength(1, ErrorMessage = "Task description cannot be empty")]
        public string Description { get; set; } = string.Empty;
    }
}

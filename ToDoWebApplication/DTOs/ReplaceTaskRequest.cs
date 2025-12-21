using System.ComponentModel.DataAnnotations;

namespace ToDoWebApplication.DTOs
{
    public class ReplaceTaskRequest
    {
        [Required(ErrorMessage = "Task description is required")]
        [MinLength(1, ErrorMessage = "Task description cannot be empty")]
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
    }
}

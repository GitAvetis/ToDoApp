using System.ComponentModel.DataAnnotations;

namespace ToDoWebApplication.Contracts.DTOs
{
    public class CreateTaskListRequest
    {
        [Required(ErrorMessage = "List name is required")]
        [MinLength(1, ErrorMessage = "List name cannot be empty")]
        public string Name { get; set; } = string.Empty;
    }
}

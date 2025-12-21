using System.ComponentModel.DataAnnotations;

namespace ToDoWebApplication.DTOs
{
    public class CreateListRequest
    {
        [Required(ErrorMessage = "List name is required")]
        [MinLength(1, ErrorMessage = "List name cannot be empty")]
        public string Name { get; set; } = string.Empty;
    }
}

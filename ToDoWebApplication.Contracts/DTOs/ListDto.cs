using ToDoWebApplication.Domain.Models;

namespace ToDoWebApplication.Contracts.DTOs
{
    public class ListDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ListType Type { get; set; }

    }
}

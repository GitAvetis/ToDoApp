namespace ToDoWebApplication.Domain.Models
{
    public enum ListType
    {
        Container = 1, // содержит списки
        Tasks = 2      // содержит задачи
    }
    public class ListModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public ListType Type { get; set; }

        public int? ParentListId { get; set; }
    }
}

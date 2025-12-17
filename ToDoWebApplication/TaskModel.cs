namespace ToDoWebApplication
{
    public class TaskModel
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        public int ListId { get; set; }
    }
}

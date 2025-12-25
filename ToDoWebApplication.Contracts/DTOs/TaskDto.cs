namespace ToDoWebApplication.Contracts.DTOs
{
    public class TaskDto
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
            //ListId в TaskDto не нужен Он уже есть в URL.
    }
}

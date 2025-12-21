namespace ToDoWebApplication.Exceptions
{
    public class TaskNotFoundException : Exception
    {
        public int ListId { get; }
        public int TaskId { get; }
        public TaskNotFoundException(int listId, int taskId)
            : base($"Task with ID {taskId} was not found in List with ID {listId}.")
        {
            ListId = listId;
            TaskId = taskId;
        }
    }
}

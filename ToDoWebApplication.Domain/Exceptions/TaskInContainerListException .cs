namespace ToDoWebApplication.Domain.Exceptions
{
    public class TaskInContainerListException : Exception
    {
        public int ListId { get; }

        public TaskInContainerListException(int listId)
            : base($"Cannot add a task to a container list with ID {listId}.")
        {
            ListId = listId;
        }
    }
}

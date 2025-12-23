namespace ToDoWebApplication.Domain.Exceptions
{
    public class ListNotFoundException: Exception
    {
        public int ListId { get; }
        public ListNotFoundException(int listId)
            : base($"List with ID {listId} was not found.")
        {
            ListId = listId;
        }
    }
}

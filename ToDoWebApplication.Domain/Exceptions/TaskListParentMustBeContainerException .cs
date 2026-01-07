namespace ToDoWebApplication.Domain.Exceptions
{
    public class TaskListParentMustBeContainerException : Exception
    {
        public int ParentListId { get; }

        public TaskListParentMustBeContainerException(int parentListId)
            : base($"A tasks list can only be created inside a container list with ID {parentListId}.")
        {
            ParentListId = parentListId;
        }
    }
}

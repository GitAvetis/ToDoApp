using ToDoWebApplication.Domain.Models;

namespace ToDoWebApplication.Application.Repositories.Interfaces
{
    public interface ITaskRepository
    {
        IReadOnlyList<TaskModel> GetAllByListId(int listId);
        TaskModel GetById(int listId, int taskId);
        TaskModel Add(int listId, string description);
        void Remove(int listId, int taskId);
        int RemoveByListId(int listId);
        void Update(int listId, int taskId, string? description, bool? isCompleted);
    }
}

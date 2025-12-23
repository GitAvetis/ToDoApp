using ToDoWebApplication.Domain.Models;

namespace ToDoWebApplication.Application.Services.Interfaces
{
    public interface ITaskService
    {
        IReadOnlyList<TaskModel> GetAllTaskByListId(int listId);
        TaskModel GetById(int listId, int taskId);
        TaskModel AddTask(int listId, string taskDescription);
        void ReplaceTask(int listId, int taskId, string description, bool isCompleted);
        void UpdateTask(int listId, int taskId, string? description, bool? isCompleted);
        void RemoveTask(int listId, int taskId);
        int RemoveByListId(int listId);
    }
}

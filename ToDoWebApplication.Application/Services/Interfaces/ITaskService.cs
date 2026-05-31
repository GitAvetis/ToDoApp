using ToDoWebApplication.Contracts.DTOs;

namespace ToDoWebApplication.Application.Services.Interfaces
{
    public interface ITaskService
    {
        IReadOnlyList<TaskDto> GetAllTaskByListId(int listId, Guid userId);
        TaskDto GetById(int listId, int taskId, Guid userId);
        TaskDto AddTask(int listId, string taskDescription, Guid userId);
        //void ReplaceTask(int listId, int taskId, string description, bool isCompleted);
        void UpdateTask(int listId, int taskId, string? description, bool? isCompleted, Guid userId);
        void RemoveTask(int listId, int taskId, Guid userId);
        int RemoveByListId(int listId, Guid userId);
    }
}

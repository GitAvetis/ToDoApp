using ToDoWebApplication.Contracts.DTOs;

namespace ToDoWebApplication.Application.Services.Interfaces
{
    public interface ITaskService
    {
        IReadOnlyList<TaskDto> GetAllTaskByListId(int listId);
        TaskDto GetById(int listId, int taskId);
        TaskDto AddTask(int listId, string taskDescription);
        //void ReplaceTask(int listId, int taskId, string description, bool isCompleted);
        void UpdateTask(int listId, int taskId, string? description, bool? isCompleted);
        void RemoveTask(int listId, int taskId);
        int RemoveByListId(int listId);
    }
}

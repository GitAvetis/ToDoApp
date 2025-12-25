using ToDoWebApplication.Contracts.DTOs;
using ToDoWebApplication.Domain.Models;

namespace ToDoWebApplication.Application.Mapping
{
    public static class TaskMappingExtensions
    {
        public static TaskDto ToDto(this TaskModel task)
        {
            return new TaskDto
            {
                Id = task.Id,
                Description = task.Description,
                IsCompleted = task.IsCompleted,
            };
        }
    }
}

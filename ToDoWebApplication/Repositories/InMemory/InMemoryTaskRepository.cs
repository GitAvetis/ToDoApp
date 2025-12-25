using ToDoWebApplication.Application.Repositories.Interfaces;
using ToDoWebApplication.Domain.Exceptions;
using ToDoWebApplication.Domain.Models;

namespace ToDoWebApplication.Repositories.InMemory
{
    public class InMemoryTaskRepository : ITaskRepository
    {
        private readonly List<TaskModel> _tasks = new();
        private int _lastTaskId;

        public IReadOnlyList<TaskModel> GetAllByListId(int listId)
            => _tasks.Where(t => t.ListId == listId).ToList();

        public TaskModel GetById(int listId, int taskId)
        {
            return _tasks.FirstOrDefault(t => t.ListId == listId && t.Id == taskId)
                ?? throw new TaskNotFoundException(listId, taskId);
        }

        public TaskModel Add(int listId, string description)
        {
            var task = new TaskModel
            {
                Id = ++_lastTaskId,
                ListId = listId,
                Description = description,
                IsCompleted = false
            };

            _tasks.Add(task);
            return task;
        }

        public void Remove(int listId, int taskId)
        {
            var task = GetById(listId, taskId);
            _tasks.Remove(task);
        }

        public int RemoveByListId(int listId)
        {
            return _tasks.RemoveAll(t => t.ListId == listId);
        }

        public void Update(int listId, int taskId, string? description, bool? isCompleted)
        {
            var task = GetById(listId, taskId);

            if (description != null)
                task.Description = description;

            if (isCompleted.HasValue)
                task.IsCompleted = isCompleted.Value;
        }
    }
}



using ToDoWebApplication.DTOs;
using ToDoWebApplication.Exceptions;
using ToDoWebApplication.Models;

namespace ToDoWebApplication.Services
{
    public class TaskService
    {
        private List<TaskModel> _listOfTasks = new List<TaskModel>();
        private readonly ListService _listService;

        public TaskService(ListService listService)
        {
            _listService = listService;
        }
        private int _lastTaskId;

        public bool ListExists(int listId)
        {
            return _listService.Exists(listId);
        }

        public IReadOnlyList<TaskModel> GetAllTaskByListId(int listId)
        {
            if (!ListExists(listId))
                throw new ListNotFoundException(listId);
            return _listOfTasks.Where(t => t.ListId == listId).ToList();
        }

        public TaskModel GetById(int listId, int taskId)
        {
            if (!ListExists(listId))
                throw new ListNotFoundException(listId);
            TaskModel task = _listOfTasks.FirstOrDefault(t => t.Id == taskId && t.ListId == listId) ??
                throw new TaskNotFoundException(listId, taskId);
            return task;
        }

        public TaskModel AddTask(int listId, string taskDescription)
        {
            if (!ListExists(listId))
                throw new ListNotFoundException(listId);
            _lastTaskId++;
            TaskModel task = new TaskModel() { Id = _lastTaskId, Description = taskDescription, IsCompleted = false, ListId = listId };
            _listOfTasks.Add(task);
            return task;
        }

        public void RemoveTask(int listId, int taskId)
        {
            TaskModel task = GetById(listId, taskId) ?? throw new TaskNotFoundException(listId, taskId);
            _listOfTasks.Remove(task);
        }

        public void ReplaceTask(int listId, int taskId, string description, bool isCompleted)
        {
            TaskModel task = GetById(listId, taskId) ?? throw new TaskNotFoundException(listId, taskId);

            task.Description = description;
            task.IsCompleted = isCompleted;
        }

        public void UpdateTask(int listId, int taskId, UpdateTaskRequest request)
        {
            TaskModel task = GetById(listId, taskId);// ?? throw new TaskNotFoundException(listId, taskId);

            if (request.Description != null)
                task.Description = request.Description;

            if (request.IsCompleted.HasValue)
                task.IsCompleted = request.IsCompleted.Value;
        }

        public int RemoveByListId(int listId)
        {
            if (!ListExists(listId))
                throw new ListNotFoundException(listId);
            return _listOfTasks.RemoveAll(t => t.ListId == listId);
        }

    }
}

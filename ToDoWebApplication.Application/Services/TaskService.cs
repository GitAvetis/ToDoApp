using ToDoWebApplication.Application.Mapping;
using ToDoWebApplication.Application.Repositories.Interfaces;
using ToDoWebApplication.Application.Services.Interfaces;
using ToDoWebApplication.Contracts.DTOs;
using ToDoWebApplication.Domain.Exceptions;
using ToDoWebApplication.Domain.Models;

namespace ToDoWebApplication.Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IListService _listService;

        public TaskService(ITaskRepository repository, IListService listService)
        {
            _taskRepository = repository;
            _listService = listService;
        }

        private void EnsureListExists(int listId)
        {
            if (!_listService.Exists(listId))
                throw new ListNotFoundException(listId);
        }

        public IReadOnlyList<TaskDto> GetAllTaskByListId(int listId)
        {
            EnsureListExists(listId);
            var tasks = _taskRepository.GetAllByListId(listId)
                .Select(task => task.ToDto()).ToList();
            return tasks;
        }

        public TaskDto GetById(int listId, int taskId)
        {
            EnsureListExists(listId);
            var task = _taskRepository.GetById(listId, taskId);
            return task.ToDto();
        }

        public TaskDto AddTask(int listId, string taskDescription)
        {
            var list = _listService.GetDomainById(listId);

            if (list.Type != ListType.Tasks)
                throw new TaskInContainerListException(listId);

            var task = _taskRepository.Add(listId, taskDescription);
            return task.ToDto();
        }

        public void RemoveTask(int listId, int taskId)
        {
            EnsureListExists(listId);
            _taskRepository.Remove(listId, taskId);
        }

        public void UpdateTask(int listId, int taskId, string? description, bool? isCompleted)
        {
            EnsureListExists(listId);
            _taskRepository.Update(listId, taskId, description, isCompleted);

        }

        public int RemoveByListId(int listId)
        {
            EnsureListExists(listId);
            return _taskRepository.RemoveByListId(listId);
        }

    }
}

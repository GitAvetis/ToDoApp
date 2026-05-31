using ToDoWebApplication.Application.Mapping;
using ToDoWebApplication.Application.Repositories.Interfaces;
using ToDoWebApplication.Application.Services.Interfaces;
using ToDoWebApplication.Contracts.DTOs;

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

        public IReadOnlyList<TaskDto> GetAllTaskByListId(int listId, Guid userId)
        {
            _listService.GetDomainById(listId, userId);
            var tasks = _taskRepository.GetAllByListId(listId)
                .Select(task => task.ToDto()).ToList();
            return tasks;
        }

        public TaskDto GetById(int listId, int taskId, Guid userId)
        {
            _listService.GetDomainById(listId, userId);
            var task = _taskRepository.GetById(listId, taskId);
            return task.ToDto();
        }

        public TaskDto AddTask(int listId, string taskDescription, Guid userId)
        {
            var list = _listService.GetDomainById(listId, userId);

            // list ВСЕГДА TaskList
            var task = _taskRepository.Add(listId, taskDescription);
            return task.ToDto();
        }

        public void RemoveTask(int listId, int taskId, Guid userId)
        {
            _listService.GetDomainById(listId, userId);
            _taskRepository.Remove(listId, taskId);
        }

        public void UpdateTask(int listId, int taskId, string? description, bool? isCompleted, Guid userId)
        {
            _listService.GetDomainById(listId, userId);
            _taskRepository.Update(listId, taskId, description, isCompleted);

        }

        public int RemoveByListId(int listId, Guid userId)
        {
            _listService.GetDomainById(listId, userId);
            return _taskRepository.RemoveByListId(listId);
        }
    }
}

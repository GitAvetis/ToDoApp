using ToDoWebApplication.Application.Services.Interfaces;
using ToDoWebApplication.Contracts.DTOs;
using ToDoWebApplication.Domain.Models;

namespace ToDoWebApplication.Application.Services
{
    public class ListApplicationService : IListApplicationService
    {
        private readonly IListService _listService;
        private readonly ITaskService _taskService;

        public ListApplicationService(IListService listService, ITaskService taskService)
        {
            _listService = listService;
            _taskService = taskService;
        }
        public ListDto CreateChildList(int parentListId, string name, ListType type)
        {
            var parent = _listService.GetDomainById(parentListId);

            if (parent.Type != ListType.Container)
                throw new InvalidOperationException(
                    "Нельзя добавлять вложенные списки в список задач"
                );

            return _listService.AddChildList(name, parentListId);
        }

        public void CascadeRemoveList(int listId)
        {
            // Remove all tasks associated with the list
            _listService.GetById(listId);// Ensure the list exists
            _taskService.RemoveByListId(listId);
            _listService.RemoveList(listId);
        }
    }
}

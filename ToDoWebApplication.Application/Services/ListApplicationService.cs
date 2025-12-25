using ToDoWebApplication.Application.Services.Interfaces;

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

        public void CascadeRemoveList(int listId)
        {
            // Remove all tasks associated with the list
            _listService.GetById(listId);// Ensure the list exists
            _taskService.RemoveByListId(listId);
            _listService.RemoveList(listId);
        }
    }
}

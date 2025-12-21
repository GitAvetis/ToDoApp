namespace ToDoWebApplication.Services
{
    public class ListApplicationService
    {
        private readonly ListService _listService;
        private readonly TaskService _taskService;

        public ListApplicationService(ListService listService, TaskService taskService)
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

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
            return _listOfTasks.Where(t => t.ListId == listId).ToList();
        }

        public TaskModel? GetById(int taskId, int listId)
        {
            return _listOfTasks.FirstOrDefault(t => t.Id == taskId && t.ListId == listId);
        }

        public TaskModel AddTask(int listId, string taskDescription)
        {
            if (_listService.Exists(listId) == false)
                throw new ArgumentException("List with the given ID does not exist.");
            _lastTaskId++;
            TaskModel task = new TaskModel() { Id = _lastTaskId, Description = taskDescription, IsCompleted = false, ListId = listId };
            _listOfTasks.Add(task);
            return task;
        }

        public bool RemoveTask(int listId, int taskId)
        {
            if (!ListExists(listId))
                return false;
            TaskModel task = _listOfTasks.FirstOrDefault(t => t.Id == taskId && t.ListId == listId);
            if (task == null)
                return false;
            _listOfTasks.Remove(task);
            return true;
        }

        public int RemoveByListId(int listId)
        {
            return _listOfTasks.RemoveAll(t => t.ListId == listId);
        }

    }
}

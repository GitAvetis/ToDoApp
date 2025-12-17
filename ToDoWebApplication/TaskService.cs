namespace ToDoWebApplication
{
    public class TaskService
    {
        public TaskService() { }
        private List<TaskModel> _listOfTasks = new List<TaskModel>();
        private int _lastTaskId;

        public List<TaskModel> GetAllTaskByListId(int listId)
        {
            return _listOfTasks.Where(t => t.ListId == listId).ToList();
        }

        public void AddTask(int listId, string taskDescription)
        {
            int taskId = ++_lastTaskId;
            TaskModel task = new TaskModel() { Id = taskId, Description = taskDescription, IsCompleted = false, ListId = listId };
            _listOfTasks.Add(task);
        }

        public bool RemoveTaskByListId(int listId)
        {
            List<TaskModel> tasks = _listOfTasks.Where(t => t.ListId == listId).ToList();
            if (!tasks.Any())
                return false;
            foreach (var task in tasks)
                _listOfTasks.Remove(task);
            return true;
        }

        public bool RemoveTaskByTaskId(int taskId)
        {
            TaskModel task = _listOfTasks.FirstOrDefault(t => t.Id == taskId);
            if (task == null)
                return false;

            _listOfTasks.Remove(task);
            return true;
        }
    }
}

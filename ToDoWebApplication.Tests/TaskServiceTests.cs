using ToDoWebApplication.Application.Services;
using ToDoWebApplication.Domain.Exceptions;
using ToDoWebApplication.Domain.Models;
using ToDoWebApplication.Repositories.InMemory;

namespace ToDoWebApplication.Tests
{
    public class TaskServiceTests
    {
        private readonly ListService _listService;
        private readonly TaskService _taskService;

        public TaskServiceTests()
        {
            var listRepo = new InMemoryListRepository();
            var taskRepo = new InMemoryTaskRepository();

            _listService = new ListService(listRepo);
            _taskService = new TaskService(taskRepo, _listService);
        }

        [Fact]
        public void AddTask_ShouldReturnNewTask()
        {
            var container = _listService.AddRootList("Root", ListType.Container);

            var taskList = _listService.AddChildList("My Tasks", container.Id);

            var task = _taskService.AddTask(taskList.Id, "My Task");

            Assert.NotNull(task);
            Assert.Equal("My Task", task.Description);
            Assert.False(task.IsCompleted);
        }

        [Fact]
        public void AddTask_ToContainerList_ShouldThrow()
        {
            var list = _listService.AddRootList("My List", Domain.Models.ListType.Container);

            Assert.Throws<TaskInContainerListException>(() => _taskService.AddTask(list.Id, "Some task"));
        }

        [Fact]
        public void GetById_NonExistentTask_ShouldThrow()
        {
            var container = _listService.AddRootList("Root", ListType.Container);

            var taskList = _listService.AddChildList("My Tasks", container.Id);

            Assert.Throws<TaskNotFoundException>(() =>
                _taskService.GetById(taskList.Id, 999));
        }

        [Fact]
        public void RemoveTask_ShouldRemoveSuccessfully()
        {
            var container = _listService.AddRootList("Root", ListType.Container);

            var taskList = _listService.AddChildList("My Tasks", container.Id);
            var task = _taskService.AddTask(taskList.Id, "Task to remove");

            _taskService.RemoveTask(taskList.Id, task.Id);

            Assert.Throws<TaskNotFoundException>(() =>
                _taskService.GetById(taskList.Id, task.Id));
        }

        [Fact]
        public void UpdateTask_ShouldModifyTaskCorrectly()
        {
            var container = _listService.AddRootList("Root", ListType.Container);

            var taskList = _listService.AddChildList("My Tasks", container.Id);
            var task = _taskService.AddTask(taskList.Id, "Old Description");

            _taskService.UpdateTask(taskList.Id, task.Id, "New Description", true);

            var updated = _taskService.GetById(taskList.Id, task.Id);
            Assert.Equal("New Description", updated.Description);
            Assert.True(updated.IsCompleted);
        }

        [Fact]
        public void GetAllTaskByListId_ShouldReturnOnlyTasksForThatList()
        {
            var container = _listService.AddRootList("Root", ListType.Container);
            var list1 = _listService.AddChildList("Tasks1", container.Id);
            var list2 = _listService.AddChildList("Tasks2", container.Id);

            var task1 = _taskService.AddTask(list1.Id, "Task 1");
            var task2 = _taskService.AddTask(list2.Id, "Task 2");

            var tasksForList1 = _taskService.GetAllTaskByListId(list1.Id);

            Assert.Single(tasksForList1);
            Assert.Equal(task1.Id, tasksForList1[0].Id);
        }
    }
}

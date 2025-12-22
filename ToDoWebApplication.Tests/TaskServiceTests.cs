using ToDoWebApplication.DTOs;
using ToDoWebApplication.Exceptions;
using ToDoWebApplication.Services;
namespace ToDoWebApplication.Tests
{

    public class TaskServiceTests
    {
        private readonly ListService _listService;
        private readonly TaskService _taskService;

        public TaskServiceTests()
        {
            _listService = new ListService();
            _taskService = new TaskService(_listService);
        }

        [Fact]
        public void AddTask_ShouldReturnNewTask()
        {
            // Arrange
            var list = _listService.AddList("My List");
            
            // Act
            var task = _taskService.AddTask(list.Id, "My Task");

            // Assert
            Assert.NotNull(task);
            Assert.Equal("My Task", task.Description);
            Assert.False(task.IsCompleted);
            Assert.Equal(list.Id, task.ListId);
        }

        [Fact]
        public void GetById_NonExistentTask_ShouldThrow()
        {
            var list = _listService.AddList("My List");

            Assert.Throws<TaskNotFoundException>(() => _taskService.GetById(list.Id, 99));
        }

        [Fact]
        public void RemoveTask_ShouldRemoveSuccessfully()
        {
            var list = _listService.AddList("My List");
            var task = _taskService.AddTask(list.Id, "Task to remove");

            _taskService.RemoveTask(list.Id, task.Id);

            Assert.Throws<TaskNotFoundException>(() => _taskService.GetById(list.Id, task.Id));
        }

        [Fact]
        public void UpdateTask_ShouldModifyTaskCorrectly()
        {
            var list = _listService.AddList("My List");
            var task = _taskService.AddTask(list.Id, "Old Description");

            _taskService.UpdateTask(list.Id, task.Id, new UpdateTaskRequest
            {
                Description = "New Description",
                IsCompleted = true
            });

            var updated = _taskService.GetById(list.Id, task.Id);
            Assert.Equal("New Description", updated.Description);
            Assert.True(updated.IsCompleted);
        }

        [Fact]
        public void GetAllTaskByListId_ShouldReturnOnlyTasksForList()
        {
            var list1 = _listService.AddList("List 1");
            var list2 = _listService.AddList("List 2");

            var task1 = _taskService.AddTask(list1.Id, "Task 1");
            var task2 = _taskService.AddTask(list2.Id, "Task 2");

            var tasksForList1 = _taskService.GetAllTaskByListId(list1.Id);

            Assert.Single(tasksForList1);
            Assert.Equal(task1.Id, tasksForList1[0].Id);
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using ToDoWebApplication.DTOs;
using ToDoWebApplication.Models;
using ToDoWebApplication.Services;

namespace ToDoWebApplication.Controllers
{
    [ApiController]
    [Route("lists/{listId}/tasks")]
    public class ListTasksController : ControllerBase
    {
        private readonly TaskService _taskService;
        private readonly ListService _listService;
        public ListTasksController(TaskService taskService, ListService listService)
        {
            _taskService = taskService;
            _listService = listService;
        }

        [HttpGet]
        public IActionResult GetTasks(int listId)
        {
            if (!_listService.Exists(listId))
                return NotFound($"List {listId} not found");
            return Ok(_taskService.GetAllTaskByListId(listId));
        }

        [HttpGet("{taskId}")]
        public IActionResult GetTask(int listId, int taskId)
        {
            TaskModel? task = _taskService.GetById(listId, taskId);
            if (task == null)
                return NotFound($"Task {taskId} not found in list {listId}");
            return Ok(task);
        }

        [HttpPost]
        public IActionResult CreateTask(int listId, [FromBody] CreateTaskRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Description))//Проверка на пустое или состоящее из пробелов описание задачи.
            {
                return BadRequest("List name cannot be empty.");//Возвращает статус 400 Bad Request с сообщением об ошибке.
            }
            try
            {
                var task = _taskService.AddTask(listId, request.Description);
                return CreatedAtAction(nameof(GetTask), new { listId, taskId = task.Id }, task);//Возвращает статус 201 Created с информацией о созданном ресурсе.
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPatch("{taskId}")]
        public IActionResult UpdateTask(int listId, int taskId, [FromBody] UpdateTaskRequest request)
        {
            var task = _taskService.GetById(listId, taskId);
            if (task == null)
            {
                return NotFound($"Task {taskId} not found in List {listId}");
            }

            if (request.Description != null)
                task.Description = request.Description;

            if (request.IsCompleted.HasValue)
                task.IsCompleted = request.IsCompleted.Value;
            return NoContent();
        }

        [HttpDelete]
        public IActionResult DeleteAllTasks(int listId)
        {
            if (_listService.Exists(listId) == false)
                return NotFound($"List {listId} not found");
            _taskService.RemoveByListId(listId);
            return NoContent();
        }

        [HttpDelete("{taskId}")]
        public IActionResult DeleteTask(int listId, int taskId)
        {
            if (!_taskService.ListExists(listId))
                return NotFound($"List {listId} not found");
            bool removed = _taskService.RemoveTask(listId, taskId);
            if (!removed)
                return NotFound($"There is no task with ID {taskId}");
            return NoContent();
        }
    }
}

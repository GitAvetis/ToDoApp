using Microsoft.AspNetCore.Mvc;
using ToDoWebApplication.DTOs;
using ToDoWebApplication.Exceptions;
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
            return Ok(_taskService.GetAllTaskByListId(listId));
        }

        [HttpGet("{taskId}")]
        public IActionResult GetTask(int listId, int taskId)
        {
            TaskModel? task = _taskService.GetById(listId, taskId);
            return Ok(task);
        }

        [HttpPost]
        public IActionResult CreateTask(int listId, [FromBody] CreateTaskRequest request)
        {
            //ASP.NET вернёт свой ValidationProblemDetails.
            var task = _taskService.AddTask(listId, request.Description);
            return CreatedAtAction(nameof(GetTask), new { listId, taskId = task.Id }, task);//Возвращает статус 201 Created с информацией о созданном ресурсе.

        }

        [HttpPut("{taskId}")]
        public IActionResult ReplaceTask(int listId, int taskId, [FromBody] ReplaceTaskRequest request)
        {
            _taskService.ReplaceTask(listId, taskId, request.Description, request.IsCompleted);
            return NoContent();
        }

        [HttpPatch("{taskId}")]
        public IActionResult UpdateTask(int listId, int taskId, [FromBody] UpdateTaskRequest request)
        {
            _taskService.UpdateTask(listId, taskId, request);
            return NoContent();
        }

        [HttpDelete]
        public IActionResult DeleteAllTasks(int listId)
        {
            _taskService.RemoveByListId(listId);
            return NoContent();
        }

        [HttpDelete("{taskId}")]
        public IActionResult DeleteTask(int listId, int taskId)
        {
            _taskService.RemoveTask(listId, taskId);
            return NoContent();
        }
    }
}

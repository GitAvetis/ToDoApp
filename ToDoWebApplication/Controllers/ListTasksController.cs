using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ToDoWebApplication.Application.Services.Interfaces;
using ToDoWebApplication.Contracts.DTOs;

namespace ToDoWebApplication.Controllers
{
    [Authorize]
    [ApiController]
    [Route("lists/{listId}/tasks")]
    public class ListTasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public ListTasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        private Guid GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.Parse(userIdClaim);
        }
        [HttpGet]
        public IActionResult GetTasks(int listId)
        {
            var userId = GetCurrentUserId();
            var tasks = _taskService.GetAllTaskByListId(listId, userId);
            return Ok(tasks);
        }

        [HttpGet("{taskId}")]
        public IActionResult GetTask(int listId, int taskId)
        {
            var userId = GetCurrentUserId();
            var taskDto = _taskService.GetById(listId, taskId, userId);
            return Ok(taskDto);
        }

        [HttpPost]
        public IActionResult CreateTask(int listId, [FromBody] CreateTaskRequest request)
        {
            var userId = GetCurrentUserId();
            //ASP.NET вернёт свой ValidationProblemDetails.
            var taskDto = _taskService.AddTask(listId, request.Description, userId);

            return CreatedAtAction(nameof(GetTask), new { listId, taskId = taskDto.Id }, taskDto);//Возвращает статус 201 Created с информацией о созданном ресурсе.

        }

        [HttpPatch("{taskId}")]
        public IActionResult UpdateTask(int listId, int taskId, [FromBody] UpdateTaskRequest request)
        {
            var userId = GetCurrentUserId();
            _taskService.UpdateTask(listId, taskId, request.Description, request.IsCompleted, userId);
            return NoContent();
        }

        [HttpDelete]
        public IActionResult DeleteAllTasks(int listId)
        {
            var userId = GetCurrentUserId();
            _taskService.RemoveByListId(listId, userId);
            return NoContent();
        }

        [HttpDelete("{taskId}")]
        public IActionResult DeleteTask(int listId, int taskId)
        {
            var userId = GetCurrentUserId();
            _taskService.RemoveTask(listId, taskId, userId);
            return NoContent();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using ToDoWebApplication.Application.Services.Interfaces;
using ToDoWebApplication.Contracts.DTOs;

namespace ToDoWebApplication.Controllers
{
    [ApiController]
    [Route("lists/{listId}/tasks")]
    public class ListTasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public ListTasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public IActionResult GetTasks(int listId)
        {
            var tasks = _taskService.GetAllTaskByListId(listId);
            return Ok(tasks);
        }

        [HttpGet("{taskId}")]
        public IActionResult GetTask(int listId, int taskId)
        {
            var taskDto = _taskService.GetById(listId, taskId);
            return Ok(taskDto);
        }

        [HttpPost]
        public IActionResult CreateTask(int listId, [FromBody] CreateTaskRequest request)
        {
            //ASP.NET вернёт свой ValidationProblemDetails.
            var taskDto = _taskService.AddTask(listId, request.Description);

            return CreatedAtAction(nameof(GetTask), new { listId, taskId = taskDto.Id }, taskDto);//Возвращает статус 201 Created с информацией о созданном ресурсе.

        }

        //[HttpPut("{taskId}")]
        //public IActionResult ReplaceTask(int listId, int taskId, [FromBody] ReplaceTaskRequest request)
        //{
        //    _taskService.ReplaceTask(listId, taskId, request.Description, request.IsCompleted);
        //    return NoContent();
        //}

        [HttpPatch("{taskId}")]
        public IActionResult UpdateTask(int listId, int taskId, [FromBody] UpdateTaskRequest request)
        {
            _taskService.UpdateTask(listId, taskId, request.Description, request.IsCompleted);
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

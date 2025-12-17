using Microsoft.AspNetCore.Mvc;

namespace ToDoWebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TasksController : ControllerBase
    {
        private static List<TaskModel> _tasks = new List<TaskModel>();
        private static int _nextId = 1;

        [HttpGet("{id}")]
        public IEnumerable<TaskModel> GetTasks(int id)
        {
            var tasks = _tasks.Where(t => t.ListId == id);
            return tasks;
        }

        [HttpPost("{listId}")]
        public IActionResult CreateTask(int listId, [FromBody] TaskModel newTask)//Этот атрибут говорит ASP.NET Core, что объект newList нужно получить из тела HTTP-запроса (JSON).
        {
            newTask.Id = _nextId++;
            newTask.ListId = listId;
            _tasks.Add(newTask);
            return CreatedAtAction(nameof(GetTasks), new { id = listId }, newTask);//Возвращает статус 201 Created с информацией о созданном ресурсе.
        }

        [HttpPatch("{id}")]
        public IActionResult UpdateTask(int id, [FromBody] TaskModel updatedTask)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
            {
                return NotFound();
            }
            task.Description = updatedTask.Description;
            task.IsCompleted = updatedTask.IsCompleted;
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTask(int id)
        {
            var task = _tasks.FirstOrDefault(l => l.Id == id);
            if (task == null)
            {
                return NotFound();
            }
            _tasks.Remove(task);
            return NoContent();
        }
    }
}

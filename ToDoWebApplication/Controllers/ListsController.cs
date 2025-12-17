using Microsoft.AspNetCore.Mvc;

namespace ToDoWebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class ListsController : ControllerBase
    {
        private static List<ListModel> _lists = new List<ListModel>();
        private static int _nextId = 1;
        [HttpGet]
        public IEnumerable<ListModel> GetLists()
        {
            return _lists;
        }

        [HttpPost]
        public IActionResult CreateList([FromBody] ListModel newList)//Этот атрибут говорит ASP.NET Core, что объект newList нужно получить из тела HTTP-запроса (JSON).
        {
            newList.Id = _nextId++;
            _lists.Add(newList);
            return CreatedAtAction(nameof(GetLists), new { id = newList.Id }, newList);//Возвращает статус 201 Created с информацией о созданном ресурсе.
          //  return Ok(newList);//Возвращает статус 200 OK с созданным ресурсом в теле ответа.
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteList(int id)
        {
            var list = _lists.FirstOrDefault(l => l.Id == id);
            if (list == null)
            {
                return NotFound();
            }
            _lists.Remove(list);
            return NoContent();
        }
    }
}

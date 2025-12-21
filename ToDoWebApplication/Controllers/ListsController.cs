using Microsoft.AspNetCore.Mvc;
using ToDoWebApplication.DTOs;
using ToDoWebApplication.Models;
using ToDoWebApplication.Services;

namespace ToDoWebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class ListsController : ControllerBase
    {
        private readonly ListService _listService;
        private readonly ListApplicationService _listApplicationService;


        public ListsController(ListService listService, ListApplicationService listApplicationService)
        {
            _listService = listService;
            _listApplicationService = listApplicationService;
        }

        [HttpGet]
        public IActionResult GetLists()
        {
            return Ok(_listService.GetAll());
        }

        [HttpGet("{listId}")]
        public IActionResult GetList(int listId)
        {
            ListModel list = _listService.GetById(listId);
            return Ok(list);
        }

        [HttpPost]
        public IActionResult CreateList([FromBody] CreateListRequest request)//Этот атрибут говорит ASP.NET Core, что объект newList нужно получить из тела HTTP-запроса (JSON).
        {
            var list = _listService.AddList(request.Name);
            return CreatedAtAction(nameof(GetList), new { listId = list.Id }, list);//Возвращает статус 201 Created с информацией о созданном ресурсе.
        }

        [HttpDelete("{listId}")]
        public IActionResult DeleteList(int listId)
        {
            _listApplicationService.CascadeRemoveList(listId);
            return NoContent();
        }
    }
}

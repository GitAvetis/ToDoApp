using Microsoft.AspNetCore.Mvc;
using ToDoWebApplication.Application.Services.Interfaces;
using ToDoWebApplication.Contracts.DTOs;

namespace ToDoWebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class ListsController : ControllerBase
    {
        private readonly IListService _listService;
        private readonly IListApplicationService _listApplicationService;


        public ListsController(IListService listService, IListApplicationService listApplicationService)
        {
            _listService = listService;
            _listApplicationService = listApplicationService;
        }

        [HttpGet]
        public IActionResult GetLists()
        {
            var lists = _listService.GetAll();
            return Ok(lists);
        }

        [HttpGet("{listId}")]
        public IActionResult GetList(int listId)
        {
            ListDto list = _listService.GetById(listId);
            return Ok(list);
        }

        [HttpPost]
        public IActionResult CreateList([FromBody] CreateListRequest request)//Этот атрибут говорит ASP.NET Core, что объект newList нужно получить из тела HTTP-запроса (JSON).
        {
            ListDto list = _listService.AddList(request.Name);

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

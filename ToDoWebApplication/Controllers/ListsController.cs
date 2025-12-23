using Microsoft.AspNetCore.Mvc;
using ToDoWebApplication.Application.Services;
using ToDoWebApplication.Application.Services.Interfaces;
using ToDoWebApplication.Domain.Models;
using ToDoWebApplication.Contracts.DTOs;

namespace ToDoWebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class ListsController : ControllerBase
    {
        private readonly IListService _listService;
        private readonly ListApplicationService _listApplicationService;


        public ListsController(IListService listService, ListApplicationService listApplicationService)
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
            ListModel list = _listService.AddList(request.Name);
            ListDto listForResponse = new()
            { 
                Id = list.Id,
                Name = request.Name
            };

            return CreatedAtAction(nameof(GetList), new { listId = listForResponse.Id }, listForResponse);//Возвращает статус 201 Created с информацией о созданном ресурсе.
        }

        [HttpDelete("{listId}")]
        public IActionResult DeleteList(int listId)
        {
            _listApplicationService.CascadeRemoveList(listId);
            return NoContent();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
        private readonly TaskService _taskService;


        public ListsController(ListService listService, TaskService taskService)
        {
            _listService = listService;
            _taskService = taskService;
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
            if (list == null)
                return NotFound($"List {listId} not found");

            return Ok(list);
        }

        [HttpPost]
        public IActionResult CreateList([FromBody] CreateListRequest request)//Этот атрибут говорит ASP.NET Core, что объект newList нужно получить из тела HTTP-запроса (JSON).
        {
            if(string.IsNullOrWhiteSpace(request.Name))//Проверка на пустое или состоящее из пробелов имя списка.
            {
                return BadRequest("List name cannot be empty.");//Возвращает статус 400 Bad Request с сообщением об ошибке.
            }
            var list = _listService.AddList(request.Name);
            return CreatedAtAction(nameof(GetList), new { listId = list.Id }, list);//Возвращает статус 201 Created с информацией о созданном ресурсе.
          //  return Ok(newList);//Возвращает статус 200 OK с созданным ресурсом в теле ответа.
        }

        [HttpDelete("{listId}")]
        public IActionResult DeleteList(int listId)
        {
            if(!_listService.Exists(listId))
                return NotFound($"List {listId} not found");

            _taskService.RemoveByListId(listId);
            _listService.RemoveList(listId);
            return NoContent();
        }
    }
}

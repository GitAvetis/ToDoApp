using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ToDoWebApplication.Application.Services.Interfaces;
using ToDoWebApplication.Contracts.DTOs;

namespace ToDoWebApplication.Controllers
{
    [Authorize]
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

        private Guid GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.Parse(userIdClaim);
        }

        [HttpGet]
        public IActionResult GetLists()
        {
            var userId = GetCurrentUserId();
            var lists = _listService.GetAll(userId);
            return Ok(lists);
        }

        [HttpGet("{listId}")]
        public IActionResult GetList(int listId)
        {
            var userId = GetCurrentUserId();
            ListDto list = _listService.GetById(listId, userId);
            return Ok(list);
        }

        [HttpPost]
        public IActionResult CreateRootList([FromBody] CreateListRequest request)//Этот атрибут говорит ASP.NET Core, что объект newList нужно получить из тела HTTP-запроса (JSON).
        {
            var userId = GetCurrentUserId();
            ListDto list = _listService.AddRootList(request.Name, userId);

            return CreatedAtAction(nameof(GetList), new { listId = list.Id }, list);//Возвращает статус 201 Created с информацией о созданном ресурсе.
        }


        [HttpPost("{parentId}/children")]
        public IActionResult CreateChildSList(int parentId, [FromBody] CreateListRequest request)//Этот атрибут говорит ASP.NET Core, что объект newList нужно получить из тела HTTP-запроса (JSON).
        {
            var userId = GetCurrentUserId();
            ListDto list = _listService.AddChildList(request.Name, parentId, userId);

            return CreatedAtAction(nameof(GetList), new { listId = list.Id }, list);//Возвращает статус 201 Created с информацией о созданном ресурсе.
        }

        [HttpDelete("{listId}")]
        public IActionResult DeleteList(int listId)
        {
            var userId = GetCurrentUserId();
            _listApplicationService.CascadeRemoveList(listId, userId);
            return NoContent();
        }
    }
}

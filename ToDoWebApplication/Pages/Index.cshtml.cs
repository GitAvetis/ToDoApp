using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ToDoWebApplication.Application.Services.Interfaces;
using ToDoWebApplication.Contracts.DTOs;

namespace ToDoWebApplication.Pages
{
    /*
        видит список корневых контейнеров
        может создать новый контейнер
    */
    public class IndexModel : PageModel
    {
        private readonly IListService _listService;

        public IReadOnlyList<ListDto> Lists { get; private set; }


        [BindProperty]
        public string NewListName { get; set; }

        public IndexModel(IListService listService)
        {
            _listService = listService;
        }

        public void OnGet()
        {
            Lists = _listService.GetRootLists();
        }        
        /*
            OnGet()
            јвтоматически вызываетс€ при:
            GET /
             Ќазвание важно:
            OnGet
            OnPost
            OnPostCreate
            OnPostDelete
            ASP.NET сам маппит методы по имени
        */

        public IActionResult OnPost()
        {
            _listService.AddRootList(NewListName);
            return RedirectToPage();
        }
    }
}

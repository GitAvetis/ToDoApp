using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ToDoWebApplication.Application.Services.Interfaces;
using ToDoWebApplication.Contracts.DTOs;

namespace ToDoWebApplication.Pages
{
    public class ContainerModel : PageModel
    {
        private readonly IListService _lists;
            
        public ListDto Container {  get;private set; }
        public IReadOnlyList<ListDto> TaskLists { get; private set; }

        [BindProperty]
        public string NewListName { get; set; }

        public ContainerModel(IListService lists)
        {
            _lists = lists;
        }

        public void OnGet(int id)
        {
            Container = _lists.GetById(id);
            TaskLists = _lists.GetChildLists(id);

        }

        public IActionResult OnPostCreate(int id)
        {
            _lists.AddChildList(NewListName, id);
            return RedirectToPage(new { id });
        //здесь мы превращаем запрос: POST /Lists/Container/5
        //в ответ: 302 Found Location: / Lists / Container / 5
        }
        public IActionResult OnPostDelete(int id)
        {
            _lists.RemoveList(id);
            return RedirectToPage("/Index");
        }

    }
}

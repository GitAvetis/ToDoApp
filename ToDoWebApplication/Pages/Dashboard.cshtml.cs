using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ToDoWebApplication.Application.Services.Interfaces;
using ToDoWebApplication.Contracts.DTOs;

namespace ToDoWebApplication.Pages
{
    public class DashboardModel : PageModel
    {
        private readonly IListService _lists;
        private readonly ITaskService _tasks;
        private readonly IListApplicationService _listApplicationService;

        public IReadOnlyList<ListDto> Lists { get; private set; }
        public IReadOnlyList<TaskDto> Tasks { get; private set; }

        //public int? SelectedListId { get; private set; }

        [BindProperty]
        public string NewListName { get; set; }

        [BindProperty]
        public string NewTaskDescription { get; set; }

        [BindProperty]
        public int? SelectedListId { get; set; }

        [BindProperty]
        public int? EditingTaskId { get; set; }

        [BindProperty]
        public string? EditedTaskDescription { get; set; }

        [BindProperty]
        public int? EditingListId { get; set; }

        [BindProperty]
        public string? EditedListName { get; set; }


        public DashboardModel(IListService lists, ITaskService tasks, IListApplicationService listApplicationService)
        {
            _lists = lists;
            _tasks = tasks;
            _listApplicationService = listApplicationService;
        }

        public void OnGet(int? listId)
        {
            Lists = _lists.GetAll();

            if (!listId.HasValue)
            {
                SelectedListId = null;
                Tasks = Array.Empty<TaskDto>();
                return;
            }

            if (!_lists.Exists(listId.Value))
            {
                SelectedListId = null;
                Tasks = Array.Empty<TaskDto>();
                return;
            }

            SelectedListId = listId;
            Tasks = _tasks.GetAllTaskByListId(listId.Value);
        }

        public IActionResult OnPostCreateList()
        {
            if (string.IsNullOrWhiteSpace(NewListName))
            {
                return RedirectToPage();
            }

            _lists.AddRootList(NewListName);

            return RedirectToPage();
        }

        public IActionResult OnPostStartEditList(int listId)
        {
            EditingListId = listId;
            Lists = _lists.GetAll();

            // Если выбран этот список, показываем его задачи
            SelectedListId = listId;
            Tasks = _tasks.GetAllTaskByListId(listId);

            return Page();
        }


        public IActionResult OnPostUpdateList(int listId)
        {
            if (string.IsNullOrWhiteSpace(EditedListName))
            {
                return RedirectToPage(); // или просто не обновляем
            }

            _lists.UpdateList(listId, EditedListName);

            return RedirectToPage();
        }

        public IActionResult OnPostCreateTask()
        {
            if (SelectedListId == null || string.IsNullOrWhiteSpace(NewTaskDescription))
            {
                return RedirectToPage();
            }

            _tasks.AddTask(SelectedListId.Value, NewTaskDescription);

            return RedirectToPage(new { listId = SelectedListId });
        }

        public IActionResult OnPostToggleTask(int listId, int taskId, bool isCompleted)
        {
            _tasks.UpdateTask(
                listId,
                taskId,
                description: null,
                isCompleted: isCompleted
            );

            return RedirectToPage(new { listId });
        }

        public IActionResult OnPostStartEditTask(int taskId, int listId)
        {
            EditingTaskId = taskId;
            SelectedListId = listId;

            Lists = _lists.GetAll();
            Tasks = _tasks.GetAllTaskByListId(listId);

            return Page();
        }


        public IActionResult OnPostUpdateTask(int listId, int taskId)
        {
            if (string.IsNullOrWhiteSpace(EditedTaskDescription))
            {
                return RedirectToPage(new { listId });
            }

            _tasks.UpdateTask(
                listId,
                taskId,
                description: EditedTaskDescription,
                isCompleted: null
            );

            return RedirectToPage(new { listId });
        }

        public IActionResult OnPostDeleteList(int listId)
        {
            _listApplicationService.CascadeRemoveList(listId);
            return RedirectToPage("/Dashboard");
        }


        public IActionResult OnPostDeleteTask(int taskId, int listId)
        {
            _tasks.RemoveTask(listId, taskId);
            return RedirectToPage(new { listId });
        }
    }

}

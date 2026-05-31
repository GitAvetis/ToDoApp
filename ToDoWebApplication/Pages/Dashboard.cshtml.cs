using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ToDoWebApplication.Application.Services.Interfaces;
using ToDoWebApplication.Contracts.DTOs;

namespace ToDoWebApplication.Pages
{
    [Authorize(AuthenticationSchemes = "Cookies")]
    public class DashboardModel : PageModel
    {
        private readonly IListService _lists;
        private readonly ITaskService _tasks;
        private readonly IListApplicationService _listApplicationService;

        public IReadOnlyList<ListDto> Lists { get; set; }
        public IReadOnlyList<TaskDto> Tasks { get; set; }
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

        private Guid GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            return Guid.Parse(userIdClaim);
        }

        public void OnGet(int? listId)
        {
            var userId = GetCurrentUserId();
            Lists = _lists.GetAll(userId);

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
            Tasks = _tasks.GetAllTaskByListId(listId.Value, userId);
        }

        public IActionResult OnPostCreateList()
        {
            if (string.IsNullOrWhiteSpace(NewListName))
            {
                return RedirectToPage();
            }

            var userId = GetCurrentUserId();
            _lists.AddRootList(NewListName, userId);

            return RedirectToPage();
        }

        public IActionResult OnPostStartEditList(int listId)
        {
            EditingListId = listId;
            var userId = GetCurrentUserId();
            Lists = _lists.GetAll(userId);

            // Если выбран этот список, показываем его задачи
            SelectedListId = listId;
            Tasks = _tasks.GetAllTaskByListId(listId, userId);
            return Page();
        }


        public IActionResult OnPostUpdateList(int listId)
        {
            if (string.IsNullOrWhiteSpace(EditedListName))
            {
                return RedirectToPage(); // или просто не обновляем
            }

            var userId = GetCurrentUserId();
            _lists.UpdateList(listId, EditedListName, userId);

            return RedirectToPage();
        }

        public IActionResult OnPostCreateTask()
        {
            if (SelectedListId == null || string.IsNullOrWhiteSpace(NewTaskDescription))
            {
                return RedirectToPage();
            }

            var userId = GetCurrentUserId();
            _tasks.AddTask(SelectedListId.Value, NewTaskDescription, userId);

            return RedirectToPage(new { listId = SelectedListId });
        }

        public IActionResult OnPostToggleTask(int listId, int taskId, bool isCompleted)
        {
            var userId = GetCurrentUserId();

            _tasks.UpdateTask(
                listId,
                taskId,
                description: null,
                isCompleted: isCompleted,
                userId: userId
            );

            return RedirectToPage(new { listId });
        }

        public IActionResult OnPostStartEditTask(int taskId, int listId)
        {
            EditingTaskId = taskId;
            SelectedListId = listId;
            var userId = GetCurrentUserId();
            Lists = _lists.GetAll(userId);
            Tasks = _tasks.GetAllTaskByListId(listId, userId);

            return Page();
        }


        public IActionResult OnPostUpdateTask(int listId, int taskId)
        {
            if (string.IsNullOrWhiteSpace(EditedTaskDescription))
            {
                return RedirectToPage(new { listId });
            }
            var userId = GetCurrentUserId();

            _tasks.UpdateTask(
                listId,
                taskId,
                description: EditedTaskDescription,
                isCompleted: null,
                userId: userId
            );

            return RedirectToPage(new { listId });
        }

        public IActionResult OnPostDeleteList(int listId)
        {
            var userId = GetCurrentUserId();
            _listApplicationService.CascadeRemoveList(listId, userId);
            return RedirectToPage("/Dashboard");
        }


        public IActionResult OnPostDeleteTask(int taskId, int listId)
        {
            var userId = GetCurrentUserId();
            _tasks.RemoveTask(listId, taskId, userId);
            return RedirectToPage(new { listId });
        }
    }

}

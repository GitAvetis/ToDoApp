using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ToDoWebApplication.Application.Services.Interfaces;
using ToDoWebApplication.Contracts.DTOs;

namespace ToDoWebApplication.Pages.Lists
{
    public class TasksModel : PageModel
    {
        private readonly IListService _lists;
        private readonly ITaskService _tasks;

        public ListDto TaskList { get; private set; }
        public IReadOnlyList<TaskDto> Tasks { get; private set; }

        public TasksModel(IListService lists, ITaskService tasks)
        {
            _lists = lists;
            _tasks = tasks;
        }

        public void OnGet(int id)
        {
            TaskList = _lists.GetById(id);
            Tasks = _tasks.GetAllTaskByListId(id);
        }
    }
}

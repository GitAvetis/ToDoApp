using ToDoWebApplication.Application.Repositories.Interfaces;
using ToDoWebApplication.Domain.Exceptions;
using ToDoWebApplication.Domain.Models;
using ToDoWebApplication.Infrastructure.Persistence;

namespace ToDoWebApplication.Infrastructure.Repositories
{
    public class EfTaskRepository : ITaskRepository
    {

        private readonly AppDbContext _context;

        //AppDbContext - представляет базу данных в виде объектов C#.
        public EfTaskRepository(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public IReadOnlyList<TaskModel> GetAllByListId(int listId)
        {
            return _context.Tasks.Where(t => t.ListId == listId).ToList();
        }

        public TaskModel GetById(int listId, int taskId)
        {
            return _context.Tasks.FirstOrDefault(t => t.ListId == listId && t.Id == taskId)
                ?? throw new TaskNotFoundException(listId, taskId);
        }

        public TaskModel Add(int listId, string description)
        {
            var task = new TaskModel
            {
                ListId = listId,
                Description = description,
                IsCompleted = false
            };

            _context.Tasks.Add(task);
            _context.SaveChanges();
            return task;
        }

        public void Remove(int listId, int taskId)
        {
            var task = GetById(listId, taskId);
            _context.Tasks.Remove(task);
            _context.SaveChanges();
        }

        public int RemoveByListId(int listId)
        {
            var tasks = _context.Tasks.Where(t => t.ListId == listId).ToList();

             _context.Tasks.RemoveRange(tasks);
            _context.SaveChanges();
            return tasks.Count;
        }

        public void Update(int listId, int taskId, string? description, bool? isCompleted)
        {
            var task = GetById(listId, taskId);

            if (description != null)
                task.Description = description;

            if (isCompleted.HasValue)
                task.IsCompleted = isCompleted.Value;
            _context.SaveChanges();
        }
    }
}


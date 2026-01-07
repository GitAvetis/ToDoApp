using ToDoWebApplication.Application.Repositories.Interfaces;
using ToDoWebApplication.Domain.Exceptions;
using ToDoWebApplication.Domain.Models;
using ToDoWebApplication.Infrastructure.Persistence;

namespace ToDoWebApplication.Infrastructure.Repositories
{
    public class EfListRepository : IListRepository
    {
        private readonly AppDbContext _context;

        //AppDbContext - представляет базу данных в виде объектов C#.
        public EfListRepository(AppDbContext appDbContext) 
        { 
            _context = appDbContext;
        }

        public bool Exists(int listId)
        {
            return _context.Lists.Any(l => l.Id == listId);
        }

        public ListModel GetById(int listId)
        {
            return _context.Lists.FirstOrDefault(l => l.Id == listId)
            ?? throw new ListNotFoundException(listId);
        }


        public IReadOnlyList<ListModel> GetAll()
        {
            return _context.Lists.ToList();
        }


        public ListModel Add(string listName, ListType type, int? parentListId = null)
        {
            if (parentListId == 0)  
                parentListId = null; 
            var list = new ListModel 
            { 
                Name = listName, 
                Type = type, 
                ParentListId = parentListId
            };
            _context.Lists.Add(list);
            _context.SaveChanges();// синхронизирует с настоящей базой PostgreSQL.
            return list;
        }

        public void Remove(int listId)
        {
            var list = GetById(listId);
            _context.Lists.Remove(list);
            _context.SaveChanges();
        }
    }
}

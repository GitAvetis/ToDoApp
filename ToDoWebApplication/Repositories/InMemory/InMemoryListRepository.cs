using ToDoWebApplication.Application.Repositories.Interfaces;
using ToDoWebApplication.Domain.Exceptions;
using ToDoWebApplication.Domain.Models;

namespace ToDoWebApplication.Repositories.InMemory
{
    public class InMemoryListRepository : IListRepository
    {
        private readonly List<ListModel> _lists = new();
        private int _lastId;

        public bool Exists(int listId)
        {
            return _lists.Any(l => l.Id == listId);
        }

        public ListModel GetById(int listId)
        {
            return _lists.FirstOrDefault(l => l.Id == listId)
            ?? throw new ListNotFoundException(listId);
        }


        public IReadOnlyList<ListModel> GetAll()
        {
          return  _lists.AsReadOnly();
        }


        public ListModel Add(string name)
        {
            _lastId++;
            var list = new ListModel { Id = _lastId, Name = name };
            _lists.Add(list);
            return list;
        }

        public void Remove(int listId)
        {
            var list = GetById(listId);
            _lists.Remove(list);
        }
    }
}

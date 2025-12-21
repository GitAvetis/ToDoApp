using ToDoWebApplication.Exceptions;
using ToDoWebApplication.Models;

namespace ToDoWebApplication.Services
{
    public class ListService
    {
        private List<ListModel> _listOfLists = new List<ListModel>();
        private int _lastListId;


        public bool Exists(int listId)
        {
            return _listOfLists.Any(l => l.Id == listId);
        }

        public ListModel GetById(int listId)
        {
            return _listOfLists.FirstOrDefault(l => l.Id == listId) ?? throw new ListNotFoundException(listId);

        }

        public IReadOnlyList<ListModel> GetAll()
        {
            return _listOfLists.AsReadOnly();
        }

        public ListModel AddList(string listName)
        {
            _lastListId++;
            ListModel list = new ListModel() { Id = _lastListId, Name = listName };
            _listOfLists.Add(list);
            return list;
        }

        public void RemoveList(int listId)
        {
            ListModel list = GetById(listId) ?? throw new ListNotFoundException(listId);
            _listOfLists.Remove(list);
        }

    }
}

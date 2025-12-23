using ToDoWebApplication.Domain.Models;

namespace ToDoWebApplication.Application.Services.Interfaces
{
    public interface IListService
    {
        bool Exists(int listId);
        public ListModel GetById(int listId);
        public IReadOnlyList<ListModel> GetAll();
        public ListModel AddList(string listName);
        public void RemoveList(int listId);

    }
}

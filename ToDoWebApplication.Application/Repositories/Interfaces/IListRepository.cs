using ToDoWebApplication.Domain.Models;

namespace ToDoWebApplication.Application.Repositories.Interfaces
{
    public interface IListRepository
    {
        bool Exists(int listId);
        ListModel GetById(int listId);
        IReadOnlyList<ListModel> GetAll();
        ListModel Add(string listName, ListType type, int? parentListId = null);
        void Remove(int listId);
    }
}

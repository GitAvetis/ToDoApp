using ToDoWebApplication.Domain.Models;

namespace ToDoWebApplication.Application.Repositories.Interfaces
{
    public interface IListRepository
    {
        bool Exists(int listId);
        ListModel GetById(int listId);
        IReadOnlyList<ListModel> GetAll();
        ListModel Add(string listName, ListType type, Guid userId, int? parentListId = null);
        void Update(int listId, string newName);
        void Remove(int listId);
    }
}

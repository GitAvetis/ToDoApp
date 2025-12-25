using ToDoWebApplication.Domain.Models;

namespace ToDoWebApplication.Application.Repositories.Interfaces
{
    public interface IListRepository
    {
        bool Exists(int listId);
        ListModel GetById(int listId);
        IReadOnlyList<ListModel> GetAll();
        ListModel Add(string name);
        void Remove(int listId);
    }
}

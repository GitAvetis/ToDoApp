using ToDoWebApplication.Contracts.DTOs;
using ToDoWebApplication.Domain.Models;

namespace ToDoWebApplication.Application.Services.Interfaces
{
    public interface IListService
    {
        bool Exists(int listId);
        public ListDto GetById(int listId);
        public IReadOnlyList<ListDto> GetAll();
        public ListDto AddList(string listName);
        public void RemoveList(int listId);

    }
}

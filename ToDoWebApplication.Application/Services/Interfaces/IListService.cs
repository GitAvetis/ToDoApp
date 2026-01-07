using ToDoWebApplication.Contracts.DTOs;
using ToDoWebApplication.Domain.Models;

namespace ToDoWebApplication.Application.Services.Interfaces
{
    public interface IListService
    {
        bool Exists(int listId);
        public ListDto GetById(int listId);
        public ListModel GetDomainById(int listId);// for application layer only
        public IReadOnlyList<ListDto> GetAll();
        public ListDto AddRootList(string name, ListType type);
        public ListDto AddChildList( string name, int parentListId);
        //public ListDto AddList(string listName, ListType type, int? parentListId = null);
        public void RemoveList(int listId);

    }
}

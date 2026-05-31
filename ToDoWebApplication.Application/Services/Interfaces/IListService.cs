 using ToDoWebApplication.Contracts.DTOs;
using ToDoWebApplication.Domain.Models;

namespace ToDoWebApplication.Application.Services.Interfaces
{
    public interface IListService
    {
        bool Exists(int listId);
        public ListDto GetById(int listId, Guid userId);
        public ListModel GetDomainById(int listId, Guid userId);// for application layer only
        public IReadOnlyList<ListDto> GetAll(Guid userId);
        public ListDto AddRootList(string name, Guid userId);
        public ListDto AddChildList( string name, int parentListId, Guid userId);
        public void RemoveList(int listId, Guid userId);
        public IReadOnlyList<ListDto> GetRootLists(Guid userId);
        public IReadOnlyList<ListDto> GetChildLists(int parentId, Guid userId);
        public void UpdateList(int listId, string newName, Guid userId);
    }
}

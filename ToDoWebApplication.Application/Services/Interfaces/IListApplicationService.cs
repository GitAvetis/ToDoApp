using ToDoWebApplication.Contracts.DTOs;
using ToDoWebApplication.Domain.Models;

namespace ToDoWebApplication.Application.Services.Interfaces
{
    public interface IListApplicationService
    {
        ListDto CreateChildList(int parentListId, Guid userId, string name, ListType type);
        public void CascadeRemoveList(int listId, Guid userId);
    }
}

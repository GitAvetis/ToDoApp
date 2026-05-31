using ToDoWebApplication.Application.Mapping;
using ToDoWebApplication.Application.Repositories.Interfaces;
using ToDoWebApplication.Application.Services.Interfaces;
using ToDoWebApplication.Contracts.DTOs;
using ToDoWebApplication.Domain.Exceptions;
using ToDoWebApplication.Domain.Models;

namespace ToDoWebApplication.Application.Services
{
    public class ListService : IListService
    {
        private readonly IListRepository _repository;

        public ListService(IListRepository repository)
        {
            _repository = repository;
        }

        public bool Exists(int listId)
        {
            return _repository.Exists(listId);
        }

        public IReadOnlyList<ListDto> GetRootLists(Guid userId)
        {
            return _repository.GetAll()
                .Where(l => l.ParentListId == null && l.UserId == userId)
                .Select(l => l.ToDto())
                .ToList();
        }

        public IReadOnlyList<ListDto> GetChildLists(int parentId, Guid userId)
        {
            return _repository.GetAll()
                .Where(l => l.ParentListId == parentId && l.UserId == userId)
                .Select(l => l.ToDto())
                .ToList();
        }
        public ListDto GetById(int listId, Guid userId)
        {
            ListModel list = _repository.GetById(listId);
            if (list.UserId != userId)
                throw new ListNotFoundException(listId);
            return list.ToDto();
        }
        /// <summary>
        /// Только для внутреннего использования другими сервисами, не для контроллеров.
        /// </summary>
        public ListModel GetDomainById(int listId, Guid userId)
        {
            ListModel list = _repository.GetById(listId);
                if (list.UserId != userId)
                    throw new ListNotFoundException(listId);
            return list;
        }

        public IReadOnlyList<ListDto> GetAll(Guid userId)
        {
            return _repository.GetAll()
            .Where(list => list.UserId == userId)
            .Select(list => list.ToDto()).ToList();
        }

        public ListDto AddRootList(string name, Guid userId)
        {
            ListModel list = _repository.Add(name, ListType.Container, userId);
            return list.ToDto();
        }

        public ListDto AddChildList(string name, int parentListId, Guid userId)
        {
            ListModel parent = GetDomainById(parentListId, userId);
            if (parent.Type != ListType.Container)
                throw new TaskListParentMustBeContainerException(parentListId);

            ListModel list = _repository.Add(name, ListType.Tasks, userId, parentListId);
            return list.ToDto();
        }

        public void UpdateList(int listId, string newName, Guid userId)
        {
            ListModel list = GetDomainById(listId, userId);
            _repository.Update(listId, newName);
        }

        public void RemoveList(int listId, Guid userId)
        {
            ListModel list = GetDomainById(listId, userId); 
            _repository.Remove(listId);
        }
    }
}

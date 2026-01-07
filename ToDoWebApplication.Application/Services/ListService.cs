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

        public ListDto GetById(int listId)
        {
            ListModel list = _repository.GetById(listId);
            return list.ToDto();
        }
        /// <summary>
        /// Только для внутреннего использования другими сервисами, не для контроллеров.
        /// </summary>
        public ListModel GetDomainById(int listId)
        {
            return _repository.GetById(listId);
        }

        public IReadOnlyList<ListDto> GetAll()
        {
            return _repository.GetAll()
            .Select(list => list.ToDto()).ToList();
        }

        public ListDto AddRootList(string name)
        {
            ListModel list = _repository.Add(name, ListType.Container, null);
            return list.ToDto();
        }

        public ListDto AddChildList(string name, int parentListId)
        {
            ListModel parent = GetDomainById(parentListId);
            if (parent.Type != ListType.Container)
                throw new TaskListParentMustBeContainerException(parentListId);

            ListModel list = _repository.Add(name, ListType.Tasks, parentListId);
            return list.ToDto();
        }

        public void RemoveList(int listId)
        {
            _repository.Remove(listId);

        }
    }
}

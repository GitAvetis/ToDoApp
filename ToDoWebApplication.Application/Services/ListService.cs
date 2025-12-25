using ToDoWebApplication.Application.Mapping;
using ToDoWebApplication.Application.Repositories.Interfaces;
using ToDoWebApplication.Application.Services.Interfaces;
using ToDoWebApplication.Contracts.DTOs;
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

        public IReadOnlyList<ListDto> GetAll()
        {
            return _repository.GetAll()
            .Select(list => list.ToDto()).ToList();
        }

        public ListDto AddList(string name)
        {
            ListModel list = _repository.Add(name);
            return list.ToDto();
        }

        public void RemoveList(int listId)
        {
            _repository.Remove(listId);

        }
    }
}

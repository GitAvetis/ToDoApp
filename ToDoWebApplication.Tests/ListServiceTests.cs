using ToDoWebApplication.Application.Services;
using ToDoWebApplication.Application.Services.Interfaces;
using ToDoWebApplication.Domain.Exceptions;
using ToDoWebApplication.Repositories.InMemory;

namespace ToDoWebApplication.Tests
{
    public class ListServiceTests
    {
        [Fact]
        public void AddList_ShouldReturnNewList()
        {
            var repo = new InMemoryListRepository();
            var service = new ListService(repo);
            var list = service.AddList("My List");
            Assert.NotNull(list);
            Assert.Equal("My List", list.Name);
            Assert.True(list.Id > 0);
        }
        [Fact]
        public void GetById_NonExistentList_ShouldThrow()
        {
            var repo = new InMemoryListRepository();
            var service = new ListService(repo);
            Assert.Throws<ListNotFoundException>(() => service.GetById(42));
        }
        [Fact]
        public void RemoveList_ShouldRemoveSuccessfully()
        {
            var repo = new InMemoryListRepository();
            var service = new ListService(repo);
            var list = service.AddList("Temp List");
            service.RemoveList(list.Id);
            Assert.Throws<ListNotFoundException>(() => service.GetById(list.Id));
        }
        [Fact]
        public void Exists_ShouldReturnTrueIfListExists()
        {
            var repo = new InMemoryListRepository();
            var service = new ListService(repo);
            var list = service.AddList("Existing List"); 
            Assert.True(service.Exists(list.Id));

        }
        [Fact]
        public void Exists_ShouldReturnFalseIfListDoesNotExist()
        {
            var repo = new InMemoryListRepository();
            var service = new ListService(repo); 
            Assert.False(service.Exists(999));
        }
    }
}
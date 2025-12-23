using ToDoWebApplication.Domain.Exceptions;
using ToDoWebApplication.Application.Services;

namespace ToDoWebApplication.Tests
{
    public class ListServiceTests
    {
        [Fact]
        public void AddList_ShouldReturnNewList()
        {
            var service = new ListService();
            var list = service.AddList("My List");
            Assert.NotNull(list);
            Assert.Equal("My List", list.Name);
            Assert.True(list.Id > 0);
        }
        [Fact]
        public void GetById_NonExistentList_ShouldThrow()
        {
            var service = new ListService();
            Assert.Throws<ListNotFoundException>(() => service.GetById(42));
        }
        [Fact]
        public void RemoveList_ShouldRemoveSuccessfully()
        {
            var service = new ListService();
            var list = service.AddList("Temp List");
            service.RemoveList(list.Id);
            Assert.Throws<ListNotFoundException>(() => service.GetById(list.Id));
        }
        [Fact]
        public void Exists_ShouldReturnTrueIfListExists()
        {
            var service = new ListService();
            var list = service.AddList("Existing List"); Assert.True(service.Exists(list.Id));
        }
        [Fact]
        public void Exists_ShouldReturnFalseIfListDoesNotExist()
        {
            var service = new ListService(); Assert.False(service.Exists(999));
        }
    }
}
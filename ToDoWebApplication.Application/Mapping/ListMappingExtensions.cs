using ToDoWebApplication.Contracts.DTOs;
using ToDoWebApplication.Domain.Models;

namespace ToDoWebApplication.Application.Mapping
{
    public static class ListMappingExtensions
    {
        public static ListDto ToDto(this ListModel listDto)
        {
            return new ListDto
            {
                Id = listDto.Id,
                Name = listDto.Name
            };
        }
    }
}

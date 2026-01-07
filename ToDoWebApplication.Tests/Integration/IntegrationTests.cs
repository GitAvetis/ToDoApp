using System.Net;
using System.Net.Http.Json;
using ToDoWebApplication.Contracts.DTOs;
using ToDoWebApplication.Domain.Models;

namespace ToDoWebApplication.Tests.Integration
{
    public class IntegrationTests
        : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public IntegrationTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Post_Lists_WithEmptyName_ShouldReturn400()
        {
            // arrange
            var request = new CreateContainerListRequest
            {
                Name = ""
            };

            // act
            var response = await _client.PostAsJsonAsync("/lists/root", request);

            // assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Get_Lists_ShouldReturn200()
        {
            // act
            var response = await _client.GetAsync("/Lists");

            // assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Get_List_NotFound_ShouldReturn404()//HTTP -> Controller -> Service -> Exception -> Middleware -> HTTP
        {
            // arrange
            var client = _client;

            // act
            var response = await client.GetAsync("/lists/999");

            // assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Delete_NonExistentList_ShouldReturn404()
        {
            // act
            var deleteResponse = await _client.DeleteAsync($"/lists/9999");

            // assert
            Assert.Equal(HttpStatusCode.NotFound, deleteResponse.StatusCode);
        }

        [Fact]
        public async Task CascadeDelete_ListWithTasks_ShouldRemoveTasks()
        {

            // arrange
            var listRequest = new CreateContainerListRequest { Name = "List for Task", Type = ListType.Tasks };
            var taskRequest = new CreateTaskRequest { Description = "Task to Delete" };

            var postListResponse = await _client.PostAsJsonAsync("/lists/root", listRequest);
            var list = await postListResponse.Content.ReadFromJsonAsync<ListDto>();

            await _client.PostAsJsonAsync($"/lists/{list!.Id}/tasks", taskRequest);

            // act
            var deleteResponse = await _client.DeleteAsync($"/lists/{list.Id}");
            deleteResponse.EnsureSuccessStatusCode();

            var getTasksResponse = await _client.GetAsync($"/lists/{list.Id}/tasks");

            // assert
            Assert.Equal(HttpStatusCode.NotFound, getTasksResponse.StatusCode);
        }

        [Fact]
        public async Task Post_Lists_ShouldReturn201AndCreatedList()
        {
            // arrange
            var request = new CreateContainerListRequest
            {
                Name = "New Integration Test List",
                Type = ListType.Container
            };
            // act
            var response = await _client.PostAsJsonAsync("/lists/root", request);

            var createdList = await response.Content.ReadFromJsonAsync<ListDto>();

            // assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.NotNull(createdList);
            Assert.True(createdList!.Id > 0);
            Assert.Equal("New Integration Test List", createdList.Name);

        }

        [Fact]
        public async Task Get_List_ById_ShouldReturnCreatedList()
        {
            // arrange
            var request = new CreateContainerListRequest
            {
                Name = "New Integration Test List for GET",
                Type = ListType.Container
            };
            // act

            var postResponse = await _client.PostAsJsonAsync("/lists/root", request);
            postResponse.EnsureSuccessStatusCode();

            var createdList = await postResponse.Content.ReadFromJsonAsync<ListDto>();
            Assert.NotNull(createdList);
            // assert

            var getResponse = await _client.GetAsync($"/lists/{createdList!.Id}");

            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
            var fetchedList = await getResponse.Content.ReadFromJsonAsync<ListDto>();
            Assert.NotNull(fetchedList);
            Assert.Equal(createdList.Id, fetchedList!.Id);
            Assert.Equal(createdList.Name, fetchedList.Name);
            /*
                Создание DTO для запроса (CreateListRequest) — готовим данные для POST.
                POST-запрос через _client.PostAsJsonAsync → сервер получает JSON и создаёт ресурс.
                Сериализация обратно в объект (ReadFromJsonAsync<ListDto>) → проверяем, что сервер вернул правильные данные (createdList).
                GET-запрос по ID → проверяем, что ресурс реально доступен (getResponse).
                Десериализация GET-ответа в fetchedList → получаем объект на уровне программы.
                Сравнение createdList и fetchedList → гарантируем, что сервер корректно сохраняет и возвращает данные.
            */
        }

        [Fact]
        public async Task Delete_List_ShouldRemoveSuccessfully()
        {
            // arrange
            var request = new CreateContainerListRequest
            {
                Name = "New Integration Test List for Delete",
                Type = ListType.Container
            };

            // act

            var postResponse = await _client.PostAsJsonAsync("/lists/root", request);
            postResponse.EnsureSuccessStatusCode();

            var createdList = await postResponse.Content.ReadFromJsonAsync<ListDto>();
            Assert.NotNull(createdList);

            // assert

            var getResponse = await _client.GetAsync($"/lists/{createdList!.Id}");
            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
            var deleteResponse = await _client.DeleteAsync($"/lists/{createdList!.Id}");
            deleteResponse.EnsureSuccessStatusCode();
            var getResponseAfterDelete = await _client.GetAsync($"/lists/{createdList!.Id}");
            Assert.Equal(HttpStatusCode.NotFound, getResponseAfterDelete.StatusCode);

        }

        //[Fact]
        //public async Task Post_Task_ShouldReturn201AndCreatedTask()
        //{
        //    // arrange
        //    var rootListRequest = new CreateRootListRequest
        //    {
        //        Name = "List for lists of tasks",
        //        Type = ListType.Container
        //    };
        //    var postRootListResponse = await _client.PostAsJsonAsync("/lists/root", rootListRequest);
        //    var list = await postRootListResponse.Content.ReadFromJsonAsync<ListDto>();

        //    var taskRequest = new CreateTaskRequest
        //    {
        //        Description = "New Task"
        //    };
        //    // act
        //    var response = await _client.PostAsJsonAsync($"/lists/{list!.Id}/tasks", taskRequest);

        //    // assert
        //    Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        //    var createdTask = await response.Content.ReadFromJsonAsync<TaskDto>();
        //    Assert.NotNull(createdTask);
        //    Assert.Equal("New Task", createdTask!.Description);
        //    Assert.False(createdTask.IsCompleted);
        //}
        [Fact]
        public async Task Post_Task_ShouldReturn201AndCreatedTask()
        {
            // arrange
            var listRequest = new CreateContainerListRequest
            {
                Name = "List for Task",
                Type = ListType.Tasks
            };

            var taskRequest = new CreateTaskRequest
            {
                Description = "New Task"
            };

            // act
            var postListResponse = await _client.PostAsJsonAsync("/lists/root", listRequest);
            postListResponse.EnsureSuccessStatusCode();

            var list = await postListResponse.Content.ReadFromJsonAsync<ListDto>();
            Assert.NotNull(list);

            var response = await _client.PostAsJsonAsync(
                $"/lists/{list!.Id}/tasks",
                taskRequest
            );

            // assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var createdTask = await response.Content.ReadFromJsonAsync<TaskDto>();
            Assert.NotNull(createdTask);
            Assert.Equal("New Task", createdTask!.Description);
            Assert.False(createdTask.IsCompleted);
        }


        [Fact]
        public async Task Get_Tasks_ShouldReturnAllTasksForList()
        {
            // arrange
            var listRequest = new CreateContainerListRequest { Name = "List for Task", Type = ListType.Tasks };
            var taskRequest1 = new CreateTaskRequest { Description = " Task 1" };
            var taskRequest2 = new CreateTaskRequest { Description = " Task 2" };
            // act
            var postListResponse = await _client.PostAsJsonAsync("/lists/root", listRequest);
            var list = await postListResponse.Content.ReadFromJsonAsync<ListDto>();
            await _client.PostAsJsonAsync($"/lists/{list!.Id}/tasks", taskRequest1);
            await _client.PostAsJsonAsync($"/lists/{list!.Id}/tasks", taskRequest2);

            // assert
            var getResponse = await _client.GetAsync($"/lists/{list.Id}/tasks");
            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
            var tasks = await getResponse.Content.ReadFromJsonAsync<List<TaskDto>>();
            Assert.True(2 == tasks!.Count);
        }

        [Fact]
        public async Task Patch_Task_ShouldUpdateTask()
        {
            {
                // arrange
                var listRequest = new CreateContainerListRequest { Name = "List with 2 Tasks", Type = ListType.Tasks };
                var taskRequest = new CreateTaskRequest { Description = " Task 1" };
                // act
                var postListResponse = await _client.PostAsJsonAsync("/lists/root", listRequest);
                var list = await postListResponse.Content.ReadFromJsonAsync<ListDto>();

                var taskResponse = await _client.PostAsJsonAsync($"/lists/{list!.Id}/tasks", taskRequest);
                var task = await taskResponse.Content.ReadFromJsonAsync<TaskDto>();

                var patchRequest = new UpdateTaskRequest { Description = " Task 1 updated ", IsCompleted = true };
                var patchResponse = await _client.PatchAsJsonAsync($"/lists/{list.Id}/tasks/{task!.Id}", patchRequest);

                // assert
                Assert.Equal(HttpStatusCode.NoContent, patchResponse.StatusCode);

                var getResponse = await _client.GetAsync($"/lists/{list.Id}/tasks/{task.Id}");
                var updatedTask = await getResponse.Content.ReadFromJsonAsync<TaskDto>();
                Assert.Equal(" Task 1 updated ", updatedTask!.Description);
                Assert.True(updatedTask!.IsCompleted);

            }
        }

        [Fact]
        public async Task Get_NonExistentTask_ShouldReturn404()
        {
            var listRequest = new CreateContainerListRequest
            {
                Name = "List for NonExistentTask",
                Type = ListType.Container
            };
            var postListResponse = await _client.PostAsJsonAsync("/lists/root", listRequest);
            var list = await postListResponse.Content.ReadFromJsonAsync<ListDto>();

            var getResponse = await _client.GetAsync($"/lists/{list!.Id}/tasks/9999");

            Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
        }

        [Fact]
        public async Task Patch_NonExistentTask_ShouldReturn404()
        {
            var listRequest = new CreateContainerListRequest
            {
                Name = "List for NonExistentPatch",
                Type = ListType.Container
            };
            var postListResponse = await _client.PostAsJsonAsync("/lists/root", listRequest);
            var list = await postListResponse.Content.ReadFromJsonAsync<ListDto>();

            var patchRequest = new UpdateTaskRequest { Description = "Updated", IsCompleted = true };
            var patchResponse = await _client.PatchAsJsonAsync($"/lists/{list!.Id}/tasks/9999", patchRequest);

            Assert.Equal(HttpStatusCode.NotFound, patchResponse.StatusCode);
        }

        [Fact]
        public async Task Delete_NonExistentTask_ShouldReturn404()
        {
            var listRequest = new CreateContainerListRequest { Name = "List for NonExistentDeleteTask", Type = ListType.Container };
            var postListResponse = await _client.PostAsJsonAsync("/lists/root", listRequest);
            var list = await postListResponse.Content.ReadFromJsonAsync<ListDto>();

            var deleteResponse = await _client.DeleteAsync($"/lists/{list!.Id}/tasks/9999");

            Assert.Equal(HttpStatusCode.NotFound, deleteResponse.StatusCode);
        }

        [Fact]
        public async Task Post_Task_InContainerList_ShouldReturn400()
        {
            // arrange
            var listRequest = new CreateContainerListRequest
            {
                Name = "Container List",
                Type = ListType.Container // список не для задач
            };
            var postListResponse = await _client.PostAsJsonAsync("/lists/root", listRequest);
            var list = await postListResponse.Content.ReadFromJsonAsync<ListDto>();

            var taskRequest = new CreateTaskRequest { Description = "Task in wrong list" };

            // act
            var postTaskResponse = await _client.PostAsJsonAsync($"/lists/{list!.Id}/tasks", taskRequest);

            // assert
            Assert.Equal(HttpStatusCode.BadRequest, postTaskResponse.StatusCode);
        }

        [Fact]
        public async Task Post_TaskList_WithTaskListAsParent_ShouldReturn400()
        {
            // arrange
            // 1. Создаём контейнер
            var listRequest = new CreateContainerListRequest
            {
                Name = "Root Container",
                Type = ListType.Container
            };

            var containerResponse = await _client.PostAsJsonAsync("/lists/root", listRequest);
            containerResponse.EnsureSuccessStatusCode();

            var container = await containerResponse.Content.ReadFromJsonAsync<ListDto>();
            Assert.NotNull(container);

            // 2. Создаём список задач внутри контейнера
            var taskListResponse = await _client.PostAsJsonAsync(
                $"/lists/{container.Id}/children",
                new CreateTaskListRequest
                {
                    Name = "Tasks List",
                });

            taskListResponse.EnsureSuccessStatusCode();
            var taskList = await taskListResponse.Content.ReadFromJsonAsync<ListDto>();
            Assert.NotNull(taskList);

            // 3. Пытаемся создать список задач внутри списка задач (запрещено)
            var invalidRequest = new CreateTaskListRequest
            {
                Name = "Invalid Nested Tasks",
            };

            // act
            var response = await _client.PostAsJsonAsync($"/lists/{taskList.Id}/children", invalidRequest);

            // assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Post_TaskWithEmptyDescription_ShouldReturn400()
        {
            var listRequest = new CreateContainerListRequest
            {
                Name = "List for InvalidTask",
                Type = ListType.Container
            };
            var postListResponse = await _client.PostAsJsonAsync("/lists/root", listRequest);
            var list = await postListResponse.Content.ReadFromJsonAsync<ListDto>();

            var taskRequest = new CreateTaskRequest { Description = "" };
            var postTaskResponse = await _client.PostAsJsonAsync($"/lists/{list!.Id}/tasks", taskRequest);

            Assert.Equal(HttpStatusCode.BadRequest, postTaskResponse.StatusCode);
        }

        [Fact]
        public async Task Post_TaskForNonExistentList_ShouldReturn404()
        {
            var taskRequest = new CreateTaskRequest { Description = "Task for missing list" };
            var postTaskResponse = await _client.PostAsJsonAsync($"/lists/9999/tasks", taskRequest);

            Assert.Equal(HttpStatusCode.NotFound, postTaskResponse.StatusCode);
        }
    }

}

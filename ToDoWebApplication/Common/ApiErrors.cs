using Microsoft.AspNetCore.Mvc;

namespace ToDoWebApplication.Common
{
    public static class ApiErrors
    {
        public static ProblemDetails ListNotFound(int listId)
        {
            return new ProblemDetails
            {
                Title = "Invalid request",
                Status = StatusCodes.Status404NotFound,
                Detail = $"List {listId} not found",
                Extensions = { ["listId"] = listId }
            };
        }

        public static ProblemDetails InvalidListName()
        {
            return new ProblemDetails
            {
                Title = "Invalid request",
                Status = StatusCodes.Status400BadRequest,
                Detail = "List name cannot be empty."
            };
        }

        public static ProblemDetails TaskNotFound(int listId, int taskId)
        {
            return new ProblemDetails
            {
                Title = "Invalid request",
                Status = StatusCodes.Status404NotFound,
                Detail = $"Task {taskId} not found in list {listId}",
                Extensions =
                    {
                        ["listId"] = listId,
                        ["taskId"] = taskId
                    }
            };
        }

        public static ProblemDetails InvalidTaskDescription()
        {
            return new ProblemDetails
            {
                Title = "Invalid request",
                Status = StatusCodes.Status400BadRequest,
                Detail = "Description cannot be empty"
            };
        }
    }
}

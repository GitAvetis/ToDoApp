using ToDoWebApplication.Common;
using ToDoWebApplication.Domain.Exceptions;

namespace ToDoWebApplication.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)//Перехватить доменные исключения и превратить их в HTTP-ответы
        {//HttpContext — всё, что связано с запросом:URL, Headers, Body,Response
            try
            {
                await _next(context);//передаём управление дальше по конвейеру обработки запросов, если исключений не возникло.
            }
            catch (ListNotFoundException ex)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                await context.Response.WriteAsJsonAsync(ApiErrors.ListNotFound(ex.ListId));
            }
            catch (TaskNotFoundException ex)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                await context.Response.WriteAsJsonAsync(ApiErrors.TaskNotFound(ex.ListId,ex.TaskId));
            }
            catch (Exception)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync("An unexpected error occurred.");
            }
        }
    }
}

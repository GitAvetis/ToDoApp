using Microsoft.AspNetCore.Mvc;
using ToDoWebApplication.Exceptions;
using ToDoWebApplication.Services;

namespace ToDoWebApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddSingleton<ListService>();
            builder.Services.AddSingleton<TaskService>();
            builder.Services.AddSingleton<ListApplicationService>();

            builder.Services.Configure<ApiBehaviorOptions>(options =>//Настройка пользовательского ответа (ДТО не прошёл проверку).
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState//формируем словарь ошибок.
                        .Where(e => e.Value.Errors.Count > 0)
                        .ToDictionary(
                            kvp => kvp.Key,
                            kvp => kvp.Value.Errors.Select(er => er.ErrorMessage).ToArray()
                        );

                    var problemDetails = new ValidationProblemDetails(errors)//Создаём объект ValidationProblemDetails с ошибками.
                    {
                        Title = "Validation failed",
                        Status = StatusCodes.Status400BadRequest
                    };
                    return new BadRequestObjectResult(problemDetails);//Возвращаем HTTP 400 BadRequest с телом problemDetails
                };
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}

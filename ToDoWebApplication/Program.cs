using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoWebApplication.Application.Repositories.Interfaces;
using ToDoWebApplication.Application.Services;
using ToDoWebApplication.Application.Services.Interfaces;
using ToDoWebApplication.Infrastructure.Persistence;
using ToDoWebApplication.Infrastructure.Repositories;
using ToDoWebApplication.Middlewares;

namespace ToDoWebApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddSwaggerGen(); 
            //Ef
            builder.Services.AddDbContext<AppDbContext>
                (options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<IListRepository, EfListRepository>();
            builder.Services.AddScoped<ITaskRepository, EfTaskRepository>();

            builder.Services.AddScoped<IListService, ListService>();//Scoped:1 запрос = 1 консистентное состояние
            builder.Services.AddScoped<ITaskService, TaskService>();
            builder.Services.AddScoped<IListApplicationService, ListApplicationService>();

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

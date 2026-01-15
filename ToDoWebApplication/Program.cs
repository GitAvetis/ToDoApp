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

            // Controllers + Swagger
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // EF Core
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(
                    builder.Configuration.GetConnectionString("DefaultConnection")
                )
            );

            // Repositories
            builder.Services.AddScoped<IListRepository, EfListRepository>();
            builder.Services.AddScoped<ITaskRepository, EfTaskRepository>();

            // Services
            builder.Services.AddScoped<IListService, ListService>();
            builder.Services.AddScoped<ITaskService, TaskService>();
            builder.Services.AddScoped<IListApplicationService, ListApplicationService>();

            // Validation behavior
            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .ToDictionary(
                            kvp => kvp.Key,
                            kvp => kvp.Value.Errors.Select(er => er.ErrorMessage).ToArray()
                        );

                    var problemDetails = new ValidationProblemDetails(errors)
                    {
                        Title = "Validation failed",
                        Status = StatusCodes.Status400BadRequest
                    };

                    return new BadRequestObjectResult(problemDetails);
                };
            });

            builder.Services.AddRazorPages();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.Migrate();
            }


            // Pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseHttpsRedirection();
            app.UseAuthorization();

            app.MapControllers();
            app.MapRazorPages();

            app.Run();
        }
    }
}

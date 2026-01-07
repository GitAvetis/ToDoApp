using Microsoft.EntityFrameworkCore;
using ToDoWebApplication.Domain.Models;

namespace ToDoWebApplication.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public DbSet<ListModel> Lists => Set<ListModel>();//переменные для таблиц из базы
        public DbSet<TaskModel> Tasks => Set<TaskModel>();

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Автоматическое подключение всех конфигураций Fluent API
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}

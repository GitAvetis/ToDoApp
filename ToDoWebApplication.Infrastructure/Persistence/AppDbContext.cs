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
            modelBuilder.Entity<ListModel>(entity =>
            {
                // PK
                entity.HasKey(l => l.Id);

                // Name
                entity.Property(l => l.Name)
                      .IsRequired()
                      .HasMaxLength(200);

                // ListType (enum)
                entity.Property(l => l.Type)
                      .IsRequired()
                      .HasConversion<int>();//Без этого EF может решить хранить enum как string

                // Self-reference
                entity.HasOne(l => l.Parent)
                      .WithMany(l => l.Children)
                      .HasForeignKey(l => l.ParentListId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Индекс — полезен сразу
                entity.HasIndex(l => l.ParentListId);
            });
        }
    }
}

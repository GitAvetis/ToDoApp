using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDoWebApplication.Domain.Models;

namespace ToDoWebApplication.Infrastructure.Persistence.Configurations
{
    public class TaskConfiguration : IEntityTypeConfiguration<TaskModel>
    {
        public void Configure(EntityTypeBuilder<TaskModel> builder)
        {
            builder.ToTable("tasks");
            builder.Property(tm => tm.Id).HasColumnName("id");
            builder.Property(tm => tm.Description).HasColumnName("description");
            builder.Property(tm => tm.IsCompleted).HasColumnName("is_completed");
            builder.Property(tm => tm.ListId).HasColumnName("list_id");
            builder.HasKey(tm => tm.Id);
            builder.Property(tm => tm.Description).IsRequired().HasMaxLength(500);
            builder.Property(tm => tm.IsCompleted).IsRequired();
            builder.Property(tm => tm.ListId).IsRequired();//EF Core поддерживает целостность через foreign key, описанный в ListConfiguration
        }
    }
}

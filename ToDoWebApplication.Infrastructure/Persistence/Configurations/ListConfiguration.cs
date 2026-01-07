using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDoWebApplication.Domain.Models;

namespace ToDoWebApplication.Infrastructure.Persistence.Configurations
{
    public class ListConfiguration : IEntityTypeConfiguration<ListModel>
    {
        public void Configure(EntityTypeBuilder<ListModel> builder)
        {
            builder.ToTable("lists");
            builder.Property(lm => lm.Id).HasColumnName("id");
            builder.Property(lm => lm.Name).HasColumnName("name");
            builder.Property(lm => lm.ParentListId).HasColumnName("parent_list_id");
            builder.Property(lm => lm.Type).HasColumnName("type");
            builder.HasKey(lm=>lm.Id);
            builder.Property(lm=>lm.Name).IsRequired().HasMaxLength(200);

            // Self-reference
            builder.HasOne(l => l.Parent)
                   .WithMany(l => l.Children)
                   .HasForeignKey(l => l.ParentListId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Связь с задачами (один List = много Tasks)
            // При удалении списка удаляются все связанные задачи
            builder.HasMany<TaskModel>()   // Связь "один-ко-многим"
                   .WithOne()               // Task не имеет навигационного свойства на List
                   .HasForeignKey(t => t.ListId)  // foreign key
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(l => l.ParentListId);
        }
    }
}

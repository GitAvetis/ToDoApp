using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDoWebApplication.Domain.Models;
using ToDoWebApplication.Infrastructure.Entitys;

namespace ToDoWebApplication.Infrastructure.Persistence.Configurations
{
    public class ListConfiguration : IEntityTypeConfiguration<ListModel>
    {
        public void Configure(EntityTypeBuilder<ListModel> builder)
        {
            builder.ToTable("lists");
            builder.HasKey(lm => lm.Id);
            builder.Property(lm => lm.Id).HasColumnName("id");
            builder.Property(lm => lm.Name).HasColumnName("name").IsRequired().HasMaxLength(200); ;
            builder.Property(lm => lm.ParentListId).HasColumnName("parent_list_id").IsRequired(false);
            builder.Property(lm => lm.Type).HasColumnName("type").IsRequired().HasConversion<int>();

            builder.Property(l => l.UserId).HasColumnName("user_id").IsRequired();

            builder.HasOne<UserEntity>()          // у списка есть владелец-пользователь
                   .WithMany()                   // у пользователя много списков (без навигации)
                   .HasForeignKey(l => l.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
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

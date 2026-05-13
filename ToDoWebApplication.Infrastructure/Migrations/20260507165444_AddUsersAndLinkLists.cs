using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoWebApplication.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUsersAndLinkLists : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Создаём таблицу users первой
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Login = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                });

            // 2. Уникальный индекс на Login
            migrationBuilder.CreateIndex(
                name: "IX_users_Login",
                table: "users",
                column: "Login",
                unique: true);

            // 3. Вставляем служебного пользователя (будет владельцем всех старых списков)
            var adminId = Guid.NewGuid();
            migrationBuilder.Sql($@"
                    INSERT INTO ""users"" (""Id"", ""Login"", ""Password"")
                    VALUES ('{adminId}', 'admin@example.com', 'PLACEHOLDER_HASH');
                ");

            // 4. Добавляем столбец user_id как nullable (чтобы не сломать существующие записи)
            migrationBuilder.AddColumn<Guid>(
                name: "user_id",
                table: "lists",
                type: "uuid",
                nullable: true);

            // 5. Привязываем все существующие списки к служебному пользователю
            migrationBuilder.Sql($@"
                    UPDATE ""lists""
                    SET ""user_id"" = '{adminId}'
                    WHERE ""user_id"" IS NULL;
                ");

            // 6. Делаем столбец обязательным (NOT NULL)
            migrationBuilder.AlterColumn<Guid>(
                name: "user_id",
                table: "lists",
                nullable: false,
                oldNullable: true);

            // 7. Индекс на user_id для внешнего ключа
            migrationBuilder.CreateIndex(
                name: "IX_lists_user_id",
                table: "lists",
                column: "user_id");

            // 8. Внешний ключ на users
            migrationBuilder.AddForeignKey(
                name: "FK_lists_users_user_id",
                table: "lists",
                column: "user_id",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_lists_users_user_id",
                table: "lists");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropIndex(
                name: "IX_lists_user_id",
                table: "lists");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "lists");
        }
    }
}

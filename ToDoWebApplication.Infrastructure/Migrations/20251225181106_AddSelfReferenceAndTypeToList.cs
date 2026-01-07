using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoWebApplication.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSelfReferenceAndTypeToList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentListId",
                table: "lists",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "lists",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_lists_ParentListId",
                table: "lists",
                column: "ParentListId");

            migrationBuilder.AddForeignKey(
                name: "FK_lists_lists_ParentListId",
                table: "lists",
                column: "ParentListId",
                principalTable: "lists",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_lists_lists_ParentListId",
                table: "lists");

            migrationBuilder.DropIndex(
                name: "IX_lists_ParentListId",
                table: "lists");

            migrationBuilder.DropColumn(
                name: "ParentListId",
                table: "lists");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "lists");
        }
    }
}

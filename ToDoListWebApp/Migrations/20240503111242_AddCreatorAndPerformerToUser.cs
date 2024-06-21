using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoListWebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatorAndPerformerToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PerformerName",
                table: "ToDoList");

            migrationBuilder.AddColumn<int>(
                name: "CreatorId",
                table: "ToDoList",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PerformerId",
                table: "ToDoList",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ToDoList_CreatorId",
                table: "ToDoList",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_ToDoList_PerformerId",
                table: "ToDoList",
                column: "PerformerId");

            migrationBuilder.AddForeignKey(
                name: "FK_ToDoList_AspNetUsers_CreatorId",
                table: "ToDoList",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ToDoList_AspNetUsers_PerformerId",
                table: "ToDoList",
                column: "PerformerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToDoList_AspNetUsers_CreatorId",
                table: "ToDoList");

            migrationBuilder.DropForeignKey(
                name: "FK_ToDoList_AspNetUsers_PerformerId",
                table: "ToDoList");

            migrationBuilder.DropIndex(
                name: "IX_ToDoList_CreatorId",
                table: "ToDoList");

            migrationBuilder.DropIndex(
                name: "IX_ToDoList_PerformerId",
                table: "ToDoList");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "ToDoList");

            migrationBuilder.DropColumn(
                name: "PerformerId",
                table: "ToDoList");

            migrationBuilder.AddColumn<string>(
                name: "PerformerName",
                table: "ToDoList",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Todoist.Data.EF.Migrations
{
    /// <inheritdoc />
    public partial class ChangetitletonameinTodoTaskentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Tasks",
                newName: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Tasks",
                newName: "Title");
        }
    }
}

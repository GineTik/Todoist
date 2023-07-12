using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Todoist.Data.EF.Migrations
{
    /// <inheritdoc />
    public partial class AddpositionpropertytoTodoTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Position",
                table: "Tasks",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Position",
                table: "Tasks");
        }
    }
}

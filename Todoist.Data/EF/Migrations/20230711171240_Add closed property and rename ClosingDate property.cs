using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Todoist.Data.EF.Migrations
{
    /// <inheritdoc />
    public partial class AddclosedpropertyandrenameClosingDateproperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ClosingDate",
                table: "Tasks",
                newName: "DateBeforeExpiration");

            migrationBuilder.AddColumn<bool>(
                name: "IsClosed",
                table: "Tasks",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsClosed",
                table: "Tasks");

            migrationBuilder.RenameColumn(
                name: "DateBeforeExpiration",
                table: "Tasks",
                newName: "ClosingDate");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Parent.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "main");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "Categories",
                newSchema: "main");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Categories",
                schema: "main",
                newName: "Categories");
        }
    }
}

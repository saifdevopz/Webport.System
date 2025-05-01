using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Parent.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Roles",
                table: "Roles");

            migrationBuilder.RenameTable(
                name: "Roles",
                newName: "Categories");

            migrationBuilder.RenameIndex(
                name: "IX_Roles_CategoryCode",
                table: "Categories",
                newName: "IX_Categories_CategoryCode");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categories",
                table: "Categories",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Categories",
                table: "Categories");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "Roles");

            migrationBuilder.RenameIndex(
                name: "IX_Categories_CategoryCode",
                table: "Roles",
                newName: "IX_Roles_CategoryCode");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Roles",
                table: "Roles",
                column: "CategoryId");
        }
    }
}

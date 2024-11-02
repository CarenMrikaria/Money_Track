using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyTrack.Migrations
{
    /// <inheritdoc />
    public partial class updatedpost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "type",
                table: "ProjectTable",
                newName: "Type");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "ProjectTable",
                newName: "type");
        }
    }
}

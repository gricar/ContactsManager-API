using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Contact37.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateContactWithRegion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DddCode",
                table: "Contacts",
                newName: "Region_DddCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Region_DddCode",
                table: "Contacts",
                newName: "DddCode");
        }
    }
}

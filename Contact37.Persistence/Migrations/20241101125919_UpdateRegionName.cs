using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Contact37.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRegionName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Region_Name",
                table: "Contacts",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Region_Name",
                table: "Contacts");
        }
    }
}

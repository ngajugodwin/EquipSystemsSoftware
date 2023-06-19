using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ItemBookingApp_API.Migrations
{
    /// <inheritdoc />
    public partial class AddUrlAndPublicIdColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PublicId",
                table: "Items",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Items",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublicId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "Items");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ItemBookingApp_API.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusColumnAndRemoveIsApprovedAndIsActive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Organisations");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Organisations");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Organisations",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Organisations");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Organisations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "Organisations",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}

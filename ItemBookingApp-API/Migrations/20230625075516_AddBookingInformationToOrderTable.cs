using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ItemBookingApp_API.Migrations
{
    /// <inheritdoc />
    public partial class AddBookingInformationToOrderTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "BookingInformation_EndDate",
                table: "Orders",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "BookingInformation_ReturnedDate",
                table: "Orders",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "BookingInformation_StartDate",
                table: "Orders",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<int>(
                name: "BookingInformation_Status",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookingInformation_EndDate",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "BookingInformation_ReturnedDate",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "BookingInformation_StartDate",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "BookingInformation_Status",
                table: "Orders");
        }
    }
}

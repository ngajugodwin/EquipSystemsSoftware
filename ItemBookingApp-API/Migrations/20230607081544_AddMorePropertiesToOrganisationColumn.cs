using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ItemBookingApp_API.Migrations
{
    /// <inheritdoc />
    public partial class AddMorePropertiesToOrganisationColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ApprovedByUserId",
                table: "Organisations",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Organisations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "Organisations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Organisations_ApprovedByUserId",
                table: "Organisations",
                column: "ApprovedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Organisations_AspNetUsers_ApprovedByUserId",
                table: "Organisations",
                column: "ApprovedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Organisations_AspNetUsers_ApprovedByUserId",
                table: "Organisations");

            migrationBuilder.DropIndex(
                name: "IX_Organisations_ApprovedByUserId",
                table: "Organisations");

            migrationBuilder.DropColumn(
                name: "ApprovedByUserId",
                table: "Organisations");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Organisations");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Organisations");
        }
    }
}

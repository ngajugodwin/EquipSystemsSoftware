﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ItemBookingApp_API.Migrations
{
    /// <inheritdoc />
    public partial class ChangeUserTypeColumnToAccountType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserType",
                table: "AspNetUsers",
                newName: "AccountType");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AccountType",
                table: "AspNetUsers",
                newName: "UserType");
        }
    }
}

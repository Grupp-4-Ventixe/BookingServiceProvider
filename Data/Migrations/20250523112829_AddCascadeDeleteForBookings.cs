﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCascadeDeleteForBookings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingOwners_BookingOwnerAddresses_BookingAddressId",
                table: "BookingOwners");

            migrationBuilder.AddForeignKey(
                name: "FK_BookingOwners_BookingOwnerAddresses_BookingAddressId",
                table: "BookingOwners",
                column: "BookingAddressId",
                principalTable: "BookingOwnerAddresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingOwners_BookingOwnerAddresses_BookingAddressId",
                table: "BookingOwners");

            migrationBuilder.AddForeignKey(
                name: "FK_BookingOwners_BookingOwnerAddresses_BookingAddressId",
                table: "BookingOwners",
                column: "BookingAddressId",
                principalTable: "BookingOwnerAddresses",
                principalColumn: "Id");
        }
    }
}

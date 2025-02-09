using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LTBACKEND.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldPaymentDateInPayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentDate",
                table: "Payments",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentDate",
                table: "Payments");
        }
    }
}

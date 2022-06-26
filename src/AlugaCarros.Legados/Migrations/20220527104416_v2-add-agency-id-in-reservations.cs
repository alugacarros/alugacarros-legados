using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlugaCarros.Legados.Api.Migrations
{
    public partial class v2addagencyidinreservations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "IdAgency",
                table: "Reservations",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdAgency",
                table: "Reservations");
        }
    }
}

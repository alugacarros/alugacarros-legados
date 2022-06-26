using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlugaCarros.Legados.Api.Migrations
{
    public partial class v4updatevehicleagencycode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdAgency",
                table: "Vehicles");

            migrationBuilder.AddColumn<string>(
                name: "AgencyCode",
                table: "Vehicles",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AgencyCode",
                table: "Vehicles");

            migrationBuilder.AddColumn<Guid>(
                name: "IdAgency",
                table: "Vehicles",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlugaCarros.Legados.Api.Migrations
{
    public partial class v3_update_libraries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JweAlgorithm",
                table: "SecurityKeys");

            migrationBuilder.DropColumn(
                name: "JweEncryption",
                table: "SecurityKeys");

            migrationBuilder.DropColumn(
                name: "JwkType",
                table: "SecurityKeys");

            migrationBuilder.DropColumn(
                name: "JwsAlgorithm",
                table: "SecurityKeys");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiredAt",
                table: "SecurityKeys",
                type: "datetime(6)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpiredAt",
                table: "SecurityKeys");

            migrationBuilder.AddColumn<string>(
                name: "JweAlgorithm",
                table: "SecurityKeys",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "JweEncryption",
                table: "SecurityKeys",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "JwkType",
                table: "SecurityKeys",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "JwsAlgorithm",
                table: "SecurityKeys",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}

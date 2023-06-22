using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagicVilla_API.Migrations
{
    /// <inheritdoc />
    public partial class AddNombresTableUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Nombres",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaActualizacion", "FechaCreacion" },
                values: new object[] { new DateTime(2023, 6, 20, 22, 4, 2, 369, DateTimeKind.Local).AddTicks(5303), new DateTime(2023, 6, 20, 22, 4, 2, 369, DateTimeKind.Local).AddTicks(5293) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaActualizacion", "FechaCreacion" },
                values: new object[] { new DateTime(2023, 6, 20, 22, 4, 2, 369, DateTimeKind.Local).AddTicks(5308), new DateTime(2023, 6, 20, 22, 4, 2, 369, DateTimeKind.Local).AddTicks(5307) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Nombres",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaActualizacion", "FechaCreacion" },
                values: new object[] { new DateTime(2023, 6, 20, 21, 54, 6, 114, DateTimeKind.Local).AddTicks(9457), new DateTime(2023, 6, 20, 21, 54, 6, 114, DateTimeKind.Local).AddTicks(9446) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaActualizacion", "FechaCreacion" },
                values: new object[] { new DateTime(2023, 6, 20, 21, 54, 6, 114, DateTimeKind.Local).AddTicks(9461), new DateTime(2023, 6, 20, 21, 54, 6, 114, DateTimeKind.Local).AddTicks(9460) });
        }
    }
}

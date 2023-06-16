using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagicVilla_API.Migrations
{
    /// <inheritdoc />
    public partial class null2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
              name: "Ocupantes",
              table: "Villas",
              type: "int",
              nullable: true
             );

            migrationBuilder.AlterColumn<string>(
                name: "MetrosCuadrados",
                table: "Villas",
                type: "int",
                nullable: true
               );

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaActualizacion", "FechaCreacion" },
                values: new object[] { new DateTime(2023, 6, 14, 12, 7, 43, 405, DateTimeKind.Local).AddTicks(4736), new DateTime(2023, 6, 14, 12, 7, 43, 405, DateTimeKind.Local).AddTicks(4718) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaActualizacion", "FechaCreacion" },
                values: new object[] { new DateTime(2023, 6, 14, 12, 7, 43, 405, DateTimeKind.Local).AddTicks(4742), new DateTime(2023, 6, 14, 12, 7, 43, 405, DateTimeKind.Local).AddTicks(4741) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaActualizacion", "FechaCreacion" },
                values: new object[] { new DateTime(2023, 6, 14, 10, 55, 52, 337, DateTimeKind.Local).AddTicks(8688), new DateTime(2023, 6, 14, 10, 55, 52, 337, DateTimeKind.Local).AddTicks(8677) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaActualizacion", "FechaCreacion" },
                values: new object[] { new DateTime(2023, 6, 14, 10, 55, 52, 337, DateTimeKind.Local).AddTicks(8693), new DateTime(2023, 6, 14, 10, 55, 52, 337, DateTimeKind.Local).AddTicks(8692) });
        }
    }
}

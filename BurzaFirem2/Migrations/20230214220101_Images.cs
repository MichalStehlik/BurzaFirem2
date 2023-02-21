using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BurzaFirem2.Migrations
{
    public partial class Images : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Images",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111110000"),
                column: "ConcurrencyStamp",
                value: "1440ae0f-52fe-4c68-bdd4-b7a1e554131f");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222220000"),
                column: "ConcurrencyStamp",
                value: "6594e97a-1f4a-4093-9153-d2395d2888cc");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "ConcurrencyStamp", "Created", "PasswordHash", "Updated" },
                values: new object[] { "d31f2d6d-8ef2-429f-bd1a-a0ff704b6000", new DateTime(2023, 2, 14, 23, 1, 1, 85, DateTimeKind.Local).AddTicks(8022), "AQAAAAEAACcQAAAAEH88eT4sYy7DBaE7ivSDgKM3OlNd8Qwr4ceyHXTcoXkvWS3NXDk5P5TWAlzWk3zf/Q==", new DateTime(2023, 2, 14, 23, 1, 1, 85, DateTimeKind.Local).AddTicks(8050) });

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "CompanyId",
                keyValue: 1,
                columns: new[] { "Created", "Updated" },
                values: new object[] { new DateTime(2023, 2, 14, 23, 1, 1, 86, DateTimeKind.Local).AddTicks(7175), new DateTime(2023, 2, 14, 23, 1, 1, 86, DateTimeKind.Local).AddTicks(7176) });

            migrationBuilder.UpdateData(
                table: "Listing",
                keyColumn: "ListingId",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2023, 2, 14, 23, 1, 1, 86, DateTimeKind.Local).AddTicks(7151));

            migrationBuilder.UpdateData(
                table: "Listing",
                keyColumn: "ListingId",
                keyValue: 2,
                column: "Created",
                value: new DateTime(2023, 2, 14, 23, 1, 1, 86, DateTimeKind.Local).AddTicks(7159));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created",
                table: "Images");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111110000"),
                column: "ConcurrencyStamp",
                value: "4a83271a-c404-45f1-8323-a1a3e1127011");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222220000"),
                column: "ConcurrencyStamp",
                value: "008764f3-26eb-43e4-b401-de71757388b5");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "ConcurrencyStamp", "Created", "PasswordHash", "Updated" },
                values: new object[] { "3e20bada-4052-450d-b8df-525decb0b846", new DateTime(2023, 2, 9, 9, 45, 27, 455, DateTimeKind.Local).AddTicks(7198), "AQAAAAEAACcQAAAAEMe6W2+2AazsiZh/Pyd85UtNem0UqM7AYXAKeSKEwOl+frclVmUwX+vRZLUTHaHMdQ==", new DateTime(2023, 2, 9, 9, 45, 27, 455, DateTimeKind.Local).AddTicks(7242) });

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "CompanyId",
                keyValue: 1,
                columns: new[] { "Created", "Updated" },
                values: new object[] { new DateTime(2023, 2, 9, 9, 45, 27, 458, DateTimeKind.Local).AddTicks(7013), new DateTime(2023, 2, 9, 9, 45, 27, 458, DateTimeKind.Local).AddTicks(7018) });

            migrationBuilder.UpdateData(
                table: "Listing",
                keyColumn: "ListingId",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2023, 2, 9, 9, 45, 27, 458, DateTimeKind.Local).AddTicks(6936));

            migrationBuilder.UpdateData(
                table: "Listing",
                keyColumn: "ListingId",
                keyValue: 2,
                column: "Created",
                value: new DateTime(2023, 2, 9, 9, 45, 27, 458, DateTimeKind.Local).AddTicks(6955));
        }
    }
}

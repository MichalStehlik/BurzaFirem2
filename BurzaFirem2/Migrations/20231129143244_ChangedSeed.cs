using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BurzaFirem2.Migrations
{
    /// <inheritdoc />
    public partial class ChangedSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Thumbnails_Images_FileId",
                table: "Thumbnails");

            migrationBuilder.AlterColumn<Guid>(
                name: "FileId",
                table: "Thumbnails",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<byte[]>(
                name: "Blob",
                table: "Thumbnails",
                type: "varbinary(max)",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)");

            migrationBuilder.AlterColumn<int>(
                name: "Width",
                table: "Images",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Height",
                table: "Images",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111110000"),
                column: "ConcurrencyStamp",
                value: null);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222220000"),
                column: "ConcurrencyStamp",
                value: null);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "ConcurrencyStamp", "Created", "PasswordHash", "Updated" },
                values: new object[] { "32fe3be3-388c-4c31-a527-84eb98a9258a", new DateTime(2023, 11, 29, 15, 32, 44, 333, DateTimeKind.Local).AddTicks(7324), "AQAAAAIAAYagAAAAEOQ5n8nMwCUs8wby54oIUae/+tKaAbrE/9aHXCyi8BLm9LrpyJPQEDIroon8uVhsDA==", new DateTime(2023, 11, 29, 15, 32, 44, 333, DateTimeKind.Local).AddTicks(7373) });

            migrationBuilder.UpdateData(
                table: "Branches",
                keyColumn: "BranchId",
                keyValue: 1,
                column: "BackgroundColor",
                value: "#3cab68");

            migrationBuilder.UpdateData(
                table: "Branches",
                keyColumn: "BranchId",
                keyValue: 2,
                column: "BackgroundColor",
                value: "#429fe3");

            migrationBuilder.UpdateData(
                table: "Branches",
                keyColumn: "BranchId",
                keyValue: 3,
                column: "BackgroundColor",
                value: "#e34242");

            migrationBuilder.UpdateData(
                table: "Branches",
                keyColumn: "BranchId",
                keyValue: 4,
                column: "BackgroundColor",
                value: "#e3a342");

            migrationBuilder.InsertData(
                table: "Branches",
                columns: new[] { "BranchId", "BackgroundColor", "Name", "TextColor", "Visible" },
                values: new object[,]
                {
                    { 5, "#9c42e3", "Oděvnictví", "#FFFFFF", true },
                    { 6, "#e3428f", "Textilnictví", "#FFFFFF", true }
                });

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "CompanyId",
                keyValue: 1,
                columns: new[] { "Created", "Updated" },
                values: new object[] { new DateTime(2023, 11, 29, 15, 32, 44, 380, DateTimeKind.Local).AddTicks(395), new DateTime(2023, 11, 29, 15, 32, 44, 380, DateTimeKind.Local).AddTicks(396) });

            migrationBuilder.UpdateData(
                table: "Listing",
                keyColumn: "ListingId",
                keyValue: 1,
                columns: new[] { "Created", "Name" },
                values: new object[] { new DateTime(2023, 11, 29, 15, 32, 44, 380, DateTimeKind.Local).AddTicks(256), "Virtuálně" });

            migrationBuilder.UpdateData(
                table: "Listing",
                keyColumn: "ListingId",
                keyValue: 2,
                columns: new[] { "Created", "Name" },
                values: new object[] { new DateTime(2023, 11, 29, 15, 32, 44, 380, DateTimeKind.Local).AddTicks(265), "Prezenčně 2023" });

            migrationBuilder.InsertData(
                table: "Listing",
                columns: new[] { "ListingId", "Created", "Name", "Visible" },
                values: new object[] { 3, new DateTime(2023, 11, 29, 15, 32, 44, 380, DateTimeKind.Local).AddTicks(273), "Prezenčně 2024", true });

            migrationBuilder.AddForeignKey(
                name: "FK_Thumbnails_Images_FileId",
                table: "Thumbnails",
                column: "FileId",
                principalTable: "Images",
                principalColumn: "ImageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Thumbnails_Images_FileId",
                table: "Thumbnails");

            migrationBuilder.DeleteData(
                table: "Branches",
                keyColumn: "BranchId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Branches",
                keyColumn: "BranchId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Listing",
                keyColumn: "ListingId",
                keyValue: 3);

            migrationBuilder.AlterColumn<Guid>(
                name: "FileId",
                table: "Thumbnails",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte[]>(
                name: "Blob",
                table: "Thumbnails",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Width",
                table: "Images",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Height",
                table: "Images",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

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
                table: "Branches",
                keyColumn: "BranchId",
                keyValue: 1,
                column: "BackgroundColor",
                value: "#D90000");

            migrationBuilder.UpdateData(
                table: "Branches",
                keyColumn: "BranchId",
                keyValue: 2,
                column: "BackgroundColor",
                value: "#357BC2");

            migrationBuilder.UpdateData(
                table: "Branches",
                keyColumn: "BranchId",
                keyValue: 3,
                column: "BackgroundColor",
                value: "#00AA80");

            migrationBuilder.UpdateData(
                table: "Branches",
                keyColumn: "BranchId",
                keyValue: 4,
                column: "BackgroundColor",
                value: "#ECB100");

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
                columns: new[] { "Created", "Name" },
                values: new object[] { new DateTime(2023, 2, 14, 23, 1, 1, 86, DateTimeKind.Local).AddTicks(7151), "2022" });

            migrationBuilder.UpdateData(
                table: "Listing",
                keyColumn: "ListingId",
                keyValue: 2,
                columns: new[] { "Created", "Name" },
                values: new object[] { new DateTime(2023, 2, 14, 23, 1, 1, 86, DateTimeKind.Local).AddTicks(7159), "2023" });

            migrationBuilder.AddForeignKey(
                name: "FK_Thumbnails_Images_FileId",
                table: "Thumbnails",
                column: "FileId",
                principalTable: "Images",
                principalColumn: "ImageId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

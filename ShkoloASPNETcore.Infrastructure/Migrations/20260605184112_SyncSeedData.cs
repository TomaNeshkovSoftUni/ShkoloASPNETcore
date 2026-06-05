using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShkoloASPNETcore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SyncSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a1b2c3d4-e5f6-7a8b-9c0d-1e2f3a4b5c6d",
                column: "ConcurrencyStamp",
                value: "55122ea8-6932-474d-96eb-f6e80b4dbdf4");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b2c3d4e5-f67a-8b9c-0d1e-2f3a4b5c6d7e",
                column: "ConcurrencyStamp",
                value: "03df52f8-5813-40fa-80e2-6cf7d8d21b7a");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c3d4e5f6-7a8b-9c0d-1e2f-3a4b5c6d7e8f",
                column: "ConcurrencyStamp",
                value: "94bf82fa-2613-41fa-90e2-6cf7d8d21b7b");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "f7b9c1d2-e3f4-5a6b-7c8d-9e0f1a2b3c4d",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "83ef07a6-8054-4638-b75d-3571d87e0dc0", "AQAAAAIAAYagAAAAEG3g66dfj9Kklm88AAnvR6DqPrXmEUtYvPq7lzOpwXmRtzv6Q==", "bca6230f-b472-466d-a60d-4ef852923508" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a1b2c3d4-e5f6-7a8b-9c0d-1e2f3a4b5c6d",
                column: "ConcurrencyStamp",
                value: "548e2f1d-3de5-4f8b-9cd5-b36ddc0272cb");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b2c3d4e5-f67a-8b9c-0d1e-2f3a4b5c6d7e",
                column: "ConcurrencyStamp",
                value: "0f4bd0bf-41e3-4313-888c-761d41a73c14");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c3d4e5f6-7a8b-9c0d-1e2f-3a4b5c6d7e8f",
                column: "ConcurrencyStamp",
                value: "f8ae259b-610f-49b9-8720-c8a2ef23126d");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "f7b9c1d2-e3f4-5a6b-7c8d-9e0f1a2b3c4d",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a67b1867-0f51-49cd-89bf-5b857dc4622d", "AQAAAAIAAYagAAAAEKiGrFggf/3cKwxXcW+hh5CqC0tdspWnkg7P3bl/R7oeMzNvAQkqxO47DqHbKUgDLA==", "bb154878-a131-4fbc-a2cf-68456273ef4b" });
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShkoloASPNETcore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixStudentUserRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "a1b2c3d4-e5f6-7a8b-9c0d-1e2f3a4b5c6d", "f7b9c1d2-e3f4-5a6b-7c8d-9e0f1a2b3c4d" });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "f7b9c1d2-e3f4-5a6b-7c8d-9e0f1a2b3c4d");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "1", 0, "83ef07a6-8054-4638-b75d-3571d87e0dc0", "admin@shkolo.bg", true, "System", "Admin", false, null, "ADMIN@SHKOLO.BG", "ADMIN@SHKOLO.BG", "AQAAAAIAAYagAAAAEG3g66dfj9Kklm88AAnvR6DqPrXmEUtYvPq7lzOpwXmRtzv6Q==", null, false, "bca6230f-b472-466d-a60d-4ef852923508", false, "admin@shkolo.bg" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "a1b2c3d4-e5f6-7a8b-9c0d-1e2f3a4b5c6d", "1" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "a1b2c3d4-e5f6-7a8b-9c0d-1e2f3a4b5c6d", "1" });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "f7b9c1d2-e3f4-5a6b-7c8d-9e0f1a2b3c4d", 0, "83ef07a6-8054-4638-b75d-3571d87e0dc0", "admin@shkolo.bg", true, "System", "Admin", false, null, "ADMIN@SHKOLO.BG", "ADMIN@SHKOLO.BG", "AQAAAAIAAYagAAAAEG3g66dfj9Kklm88AAnvR6DqPrXmEUtYvPq7lzOpwXmRtzv6Q==", null, false, "bca6230f-b472-466d-a60d-4ef852923508", false, "admin@shkolo.bg" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "a1b2c3d4-e5f6-7a8b-9c0d-1e2f3a4b5c6d", "f7b9c1d2-e3f4-5a6b-7c8d-9e0f1a2b3c4d" });
        }
    }
}

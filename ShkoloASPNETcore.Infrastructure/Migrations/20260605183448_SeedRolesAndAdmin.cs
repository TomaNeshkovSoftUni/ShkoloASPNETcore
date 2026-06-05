using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ShkoloASPNETcore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedRolesAndAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "a1b2c3d4-e5f6-7a8b-9c0d-1e2f3a4b5c6d", "548e2f1d-3de5-4f8b-9cd5-b36ddc0272cb", "Administrator", "ADMINISTRATOR" },
                    { "b2c3d4e5-f67a-8b9c-0d1e-2f3a4b5c6d7e", "0f4bd0bf-41e3-4313-888c-761d41a73c14", "Teacher", "TEACHER" },
                    { "c3d4e5f6-7a8b-9c0d-1e2f-3a4b5c6d7e8f", "f8ae259b-610f-49b9-8720-c8a2ef23126d", "Student", "STUDENT" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "f7b9c1d2-e3f4-5a6b-7c8d-9e0f1a2b3c4d", 0, "a67b1867-0f51-49cd-89bf-5b857dc4622d", "admin@shkolo.bg", true, "System", "Admin", false, null, "ADMIN@SHKOLO.BG", "ADMIN@SHKOLO.BG", "AQAAAAIAAYagAAAAEKiGrFggf/3cKwxXcW+hh5CqC0tdspWnkg7P3bl/R7oeMzNvAQkqxO47DqHbKUgDLA==", null, false, "bb154878-a131-4fbc-a2cf-68456273ef4b", false, "admin@shkolo.bg" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "a1b2c3d4-e5f6-7a8b-9c0d-1e2f3a4b5c6d", "f7b9c1d2-e3f4-5a6b-7c8d-9e0f1a2b3c4d" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b2c3d4e5-f67a-8b9c-0d1e-2f3a4b5c6d7e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c3d4e5f6-7a8b-9c0d-1e2f-3a4b5c6d7e8f");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "a1b2c3d4-e5f6-7a8b-9c0d-1e2f3a4b5c6d", "f7b9c1d2-e3f4-5a6b-7c8d-9e0f1a2b3c4d" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a1b2c3d4-e5f6-7a8b-9c0d-1e2f3a4b5c6d");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "f7b9c1d2-e3f4-5a6b-7c8d-9e0f1a2b3c4d");
        }
    }
}

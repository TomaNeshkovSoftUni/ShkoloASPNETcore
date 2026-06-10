using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShkoloASPNETcore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialDbSetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEDk9jaudQb0s8hcrnK2b0JiryTLI0jgsIM+oUdJADPpYawsE+JrrRQwaIAHFCSUszQ==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEG3g66dfj9Kklm88AAnvR6DqPrXmEUtYvPq7lzOpwXmRtzv6Q==");
        }
    }
}

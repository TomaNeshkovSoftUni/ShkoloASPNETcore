using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShkoloASPNETcore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRemarkStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Text",
                table: "Remarks");

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "Remarks",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Issuer",
                table: "Remarks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Label",
                table: "Remarks",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                table: "Remarks");

            migrationBuilder.DropColumn(
                name: "Issuer",
                table: "Remarks");

            migrationBuilder.DropColumn(
                name: "Label",
                table: "Remarks");

            migrationBuilder.AddColumn<string>(
                name: "Text",
                table: "Remarks",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }
    }
}

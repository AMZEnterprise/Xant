using Microsoft.EntityFrameworkCore.Migrations;

namespace Xant.Persistence.Migrations
{
    public partial class AddEmailOrPhoneToContact : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Contacts");

            migrationBuilder.AddColumn<string>(
                name: "EmailOrPhoneNumber",
                table: "Contacts",
                maxLength: 256,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailOrPhoneNumber",
                table: "Contacts");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Contacts",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Contacts",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);
        }
    }
}

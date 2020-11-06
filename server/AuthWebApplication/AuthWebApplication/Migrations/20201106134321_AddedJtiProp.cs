using Microsoft.EntityFrameworkCore.Migrations;

namespace AuthWebApplication.Migrations
{
    public partial class AddedJtiProp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Jti",
                table: "AspNetUserTokens",
                type: "varchar(64)",
                maxLength: 64,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Jti",
                table: "AspNetUserTokens");
        }
    }
}

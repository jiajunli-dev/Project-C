using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Data",
                table: "Photos",
                type: "text",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "bytea",
                oldMaxLength: 20971520);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "Data",
                table: "Photos",
                type: "bytea",
                maxLength: 20971520,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}

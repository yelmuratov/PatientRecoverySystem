using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PatientRecoverySystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RecoverEntityChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "RecoveryLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "RecoveryLogs");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PatientRecoverySystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RecoverEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DoctorId",
                table: "RecoveryLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DoctorId",
                table: "RecoveryLogs");
        }
    }
}

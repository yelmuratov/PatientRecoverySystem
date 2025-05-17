using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PatientRecoverySystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRehabilitationRecord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RehabilitationRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    DoctorId = table.Column<int>(type: "int", nullable: false),
                    Plan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProgressNote = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateAssigned = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RehabilitationRecords", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RehabilitationRecords");
        }
    }
}

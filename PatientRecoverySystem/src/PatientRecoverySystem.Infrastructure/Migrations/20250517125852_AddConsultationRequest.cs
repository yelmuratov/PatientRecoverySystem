using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PatientRecoverySystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddConsultationRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConsultationRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    SymptomDescription = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    SystemAdvice = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    EscalatedToDoctor = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsultationRequests", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConsultationRequests");
        }
    }
}

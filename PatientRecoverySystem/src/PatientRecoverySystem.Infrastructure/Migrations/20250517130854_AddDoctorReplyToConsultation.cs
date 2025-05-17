using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PatientRecoverySystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDoctorReplyToConsultation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DoctorReply",
                table: "ConsultationRequests",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DoctorReply",
                table: "ConsultationRequests");
        }
    }
}

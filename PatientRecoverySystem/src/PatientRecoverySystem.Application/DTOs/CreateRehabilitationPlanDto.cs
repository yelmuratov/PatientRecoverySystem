namespace PatientRecoverySystem.Application.DTOs;

using System.ComponentModel.DataAnnotations;

public class CreateRehabilitationPlanDto
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "DoctorId must be a positive integer.")]
    public int DoctorId { get; set; }

    [Required]
    [StringLength(500, MinimumLength = 3, ErrorMessage = "Plan must be between 3 and 500 characters.")]
    public string Plan { get; set; }
}



using PatientRecoverySystem.Application.DTOs;
using PatientRecoverySystem.Domain.Entities;

namespace PatientRecoverySystem.Application.Interfaces
{
    public interface IGeminiService
    {
        Task<string> GenerateAdviceAsync(string symptomDescription);
    }
}

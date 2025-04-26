using PatientRecoverySystem.Application.DTOs;

namespace PatientRecoverySystem.Application.Interfaces;

public interface IAuthService
{
    Task<string> LoginAsync(LoginDto loginDto);
}

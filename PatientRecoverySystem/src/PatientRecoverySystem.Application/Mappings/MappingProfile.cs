using AutoMapper;
using PatientRecoverySystem.Domain.Entities;
using PatientRecoverySystem.Application.DTOs;

namespace PatientRecoverySystem.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Entity -> DTO
        CreateMap<Patient, PatientDto>().ReverseMap();
        CreateMap<Doctor, DoctorDto>().ReverseMap();
        CreateMap<RecoveryLog, RecoveryLogDto>().ReverseMap();
        
    }
}

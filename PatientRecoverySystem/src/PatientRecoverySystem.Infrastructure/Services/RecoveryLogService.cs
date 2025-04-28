using AutoMapper;
using MassTransit;
using PatientRecoverySystem.Application.DTOs;
using PatientRecoverySystem.Application.Events;
using PatientRecoverySystem.Application.Interfaces;
using PatientRecoverySystem.Domain.Entities;
using PatientRecoverySystem.Domain.Interfaces;

namespace PatientRecoverySystem.Infrastructure.Services
{
    public class RecoveryLogService : IRecoveryLogService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint; 

        public RecoveryLogService(IPatientRepository patientRepository, IMapper mapper, IPublishEndpoint publishEndpoint)
        {
            _patientRepository = patientRepository;
            _mapper = mapper;
            _publishEndpoint = publishEndpoint; 
        }

        public async Task AddRecoveryLogAsync(int patientId, RecoveryLogDto logDto)
        {
            var recoveryLog = _mapper.Map<RecoveryLog>(logDto);
            recoveryLog.Id = 0;
            recoveryLog.PatientId = patientId;

            await _patientRepository.AddRecoveryLogAsync(recoveryLog);

            if (logDto.IsEmergency)
            {
                await _publishEndpoint.Publish(new EmergencyCreatedEvent
                {
                    PatientId = patientId,
                    EmergencyType = logDto.Description ?? "Emergency Alert", // Use Description if available
                    CreatedAt = DateTime.UtcNow
                });
            }
        }

        public async Task<List<RecoveryLogDto>> GetRecoveryLogsByPatientIdAsync(int patientId)
        {
            var logs = await _patientRepository.GetRecoveryLogsByPatientIdAsync(patientId);
            return _mapper.Map<List<RecoveryLogDto>>(logs);
        }

        public async Task<RecoveryLogDto?> GetRecoveryLogByIdAsync(int id)
        {
            var log = await _patientRepository.GetRecoveryLogByIdAsync(id);
            return _mapper.Map<RecoveryLogDto>(log);
        }

        public async Task<RecoveryLogDto?> UpdateRecoveryLogAsync(int id, RecoveryLogDto dto)
        {
            var existingLog = await _patientRepository.GetRecoveryLogByIdAsync(id);
            if (existingLog == null) return null;

            existingLog.Description = dto.Description;
            existingLog.Timestamp = dto.Timestamp;
            existingLog.IsEmergency = dto.IsEmergency;
            existingLog.Temperature = dto.Temperature;
            existingLog.HeartRate = dto.HeartRate;
            existingLog.Systolic = dto.Systolic;
            existingLog.Diastolic = dto.Diastolic;
            existingLog.PainLevel = dto.PainLevel;

            var updated = await _patientRepository.UpdateRecoveryLogAsync(existingLog);
            return _mapper.Map<RecoveryLogDto>(updated);
        }

        public async Task DeleteRecoveryLogAsync(int id)
        {
            await _patientRepository.DeleteRecoveryLogAsync(id);
        }

        public async Task<List<RecoveryLogDto>> GetAllRecoveryLogsAsync()
        {
            var logs = await _patientRepository.GetAllRecoveryLogsAsync();
            return _mapper.Map<List<RecoveryLogDto>>(logs);
        }

        public async Task<List<RecoveryLogDto>> GetRecoveryLogsByDoctorIdAsync(int doctorId)
        {
            var logs = await _patientRepository.GetRecoveryLogsByDoctorIdAsync(doctorId);
            return _mapper.Map<List<RecoveryLogDto>>(logs);
        }
    }
}

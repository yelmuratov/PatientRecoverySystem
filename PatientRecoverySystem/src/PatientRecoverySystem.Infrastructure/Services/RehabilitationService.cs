using PatientRecoverySystem.Application.Interfaces;
using PatientRecoverySystem.Application.DTOs;
using PatientRecoverySystem.Domain.Entities;
using PatientRecoverySystem.Domain.Interfaces;
using PatientRecoverySystem.Application.Exceptions; // for NotFoundException

namespace PatientRecoverySystem.Infrastructure.Services
{
    public class RehabilitationService : IRehabilitationService
    {
        private readonly IRehabilitationRepository _repository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IPatientRepository _patientRepository;

        public RehabilitationService(
            IRehabilitationRepository repository,
            IDoctorRepository doctorRepository,
            IPatientRepository patientRepository)
        {
            _repository = repository;
            _doctorRepository = doctorRepository;
            _patientRepository = patientRepository;
        }

        public async Task AddRehabilitationPlanAsync(int patientId, CreateRehabilitationPlanDto dto)
        {
            var doctor = await _doctorRepository.GetByIdAsync(dto.DoctorId);
            if (doctor == null)
                throw new NotFoundException($"Doctor with ID {dto.DoctorId} does not exist.");

            var patient = await _patientRepository.GetByIdAsync(patientId);
            if (patient == null)
                throw new NotFoundException($"Patient with ID {patientId} does not exist.");

            var record = new RehabilitationRecord
            {
                PatientId = patientId,
                DoctorId = dto.DoctorId,
                Plan = dto.Plan,
                DateAssigned = DateTime.UtcNow
            };

            await _repository.AddAsync(record);
        }

        public async Task<bool> UpdateRehabilitationProgressAsync(int id, UpdateRehabilitationProgressDto dto)
        {
            return await _repository.UpdateProgressAsync(id, dto.ProgressNote);
        }

        public async Task<bool> UpdateRehabilitationPlanAsync(int id, UpdateRehabilitationPlanDto dto)
        {
            return await _repository.UpdatePlanAsync(id, dto.Plan);
        }

        public async Task<IEnumerable<RehabilitationRecord>> GetRehabilitationProgressAsync(int patientId)
        {
            return await _repository.GetByPatientIdAsync(patientId);
        }

        public async Task<bool> DeleteRehabilitationPlanAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}

using PatientRecoverySystem.Application.DTOs;
using PatientRecoverySystem.Application.Exceptions;
using PatientRecoverySystem.Application.Interfaces;
using PatientRecoverySystem.Domain.Entities;
using PatientRecoverySystem.Domain.Interfaces;

namespace PatientRecoverySystem.Infrastructure.Services
{
    public class ConsultationService : IConsultationService
    {
        private readonly IConsultationRepository _consultationRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IGeminiService _geminiService;

        public ConsultationService(
            IConsultationRepository consultationRepository,
            IPatientRepository patientRepository,
            IGeminiService geminiService)
        {
            _consultationRepository = consultationRepository;
            _patientRepository = patientRepository;
            _geminiService = geminiService;
        }

        public async Task<ConsultationResponseDto> AskConsultationAsync(int patientId, AskConsultationDto dto)
        {
            var patient = await _patientRepository.GetByIdAsync(patientId);
            if (patient == null)
                throw new NotFoundException($"Patient with ID {patientId} does not exist.");

            // 🔍 Simple escalation logic
            var escalated = dto.SymptomDescription.Contains("chest pain", StringComparison.OrdinalIgnoreCase)
                         || dto.SymptomDescription.Contains("bleeding", StringComparison.OrdinalIgnoreCase)
                         || dto.SymptomDescription.Contains("difficulty breathing", StringComparison.OrdinalIgnoreCase);

            // 🤖 Get system advice from Gemini
            string advice;
            try
            {
                advice = await _geminiService.GenerateAdviceAsync(dto.SymptomDescription);
            }
            catch
            {
                advice = "Unable to generate AI-based advice at the moment. Please monitor symptoms or contact a doctor.";
            }

            var entity = new ConsultationRequest
            {
                PatientId = patientId,
                SymptomDescription = dto.SymptomDescription,
                SystemAdvice = advice,
                EscalatedToDoctor = escalated,
                CreatedAt = DateTime.UtcNow
            };

            await _consultationRepository.AddAsync(entity);

            return new ConsultationResponseDto
            {
                Id = entity.Id,
                PatientId = entity.PatientId,
                SymptomDescription = entity.SymptomDescription,
                SystemAdvice = entity.SystemAdvice,
                EscalatedToDoctor = entity.EscalatedToDoctor,
                CreatedAt = entity.CreatedAt
            };
        }

        public async Task<IEnumerable<ConsultationResponseDto>> GetConsultationsByPatientAsync(int patientId)
        {
            var consultations = await _consultationRepository.GetByPatientIdAsync(patientId);

            return consultations.Select(c => new ConsultationResponseDto
            {
                Id = c.Id,
                PatientId = c.PatientId,
                SymptomDescription = c.SymptomDescription,
                SystemAdvice = c.SystemAdvice,
                EscalatedToDoctor = c.EscalatedToDoctor,
                CreatedAt = c.CreatedAt,
                DoctorReply = c.DoctorReply
            });
        }

        public async Task<ConsultationResponseDto?> GetByIdAsync(int id)
        {
            var c = await _consultationRepository.GetByIdAsync(id);
            if (c == null) return null;

            return new ConsultationResponseDto
            {
                Id = c.Id,
                PatientId = c.PatientId,
                SymptomDescription = c.SymptomDescription,
                SystemAdvice = c.SystemAdvice,
                EscalatedToDoctor = c.EscalatedToDoctor,
                CreatedAt = c.CreatedAt,
                DoctorReply = c.DoctorReply
            };
        }

        public async Task<bool> ReplyToConsultationAsync(int consultationId, ReplyConsultationDto dto)
        {
            var consultation = await _consultationRepository.GetByIdAsync(consultationId);
            if (consultation == null || !consultation.EscalatedToDoctor)
                return false;

            consultation.DoctorReply = dto.DoctorReply;
            await _consultationRepository.SaveChangesAsync();
            return true;
        }
    }
}

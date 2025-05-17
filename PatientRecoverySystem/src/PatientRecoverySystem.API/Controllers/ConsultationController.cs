using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PatientRecoverySystem.Application.DTOs;
using PatientRecoverySystem.Application.Interfaces;

namespace PatientRecoverySystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ConsultationController : ControllerBase
    {
        private readonly IConsultationService _consultationService;

        public ConsultationController(IConsultationService consultationService)
        {
            _consultationService = consultationService;
        }

        // POST: /api/Consultation/ask/{patientId}
        [HttpPost("ask/{patientId}")]
        public async Task<IActionResult> AskConsultation(int patientId, [FromBody] AskConsultationDto dto)
        {
            var response = await _consultationService.AskConsultationAsync(patientId, dto);
            return Ok(response);
        }

        // GET: /api/Consultation/patient/{patientId}
        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetByPatientId(int patientId)
        {
            var list = await _consultationService.GetConsultationsByPatientAsync(patientId);
            return Ok(list);
        }

        // GET: /api/Consultation/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _consultationService.GetByIdAsync(id);
            if (result == null)
                return NotFound(new { Message = $"Consultation request with ID {id} not found." });

            return Ok(result);
        }

        [HttpPut("reply/{consultationId}")]
        public async Task<IActionResult> Reply(int consultationId, [FromBody] ReplyConsultationDto dto)
        {
            var success = await _consultationService.ReplyToConsultationAsync(consultationId, dto);
            if (!success)
                return BadRequest(new { Message = "Invalid consultation ID or not escalated to doctor." });

            return Ok(new { Message = "Reply sent successfully." });
        }
    }
}

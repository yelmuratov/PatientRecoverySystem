using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PatientRecoverySystem.Application.DTOs;
using PatientRecoverySystem.Application.Interfaces;

namespace PatientRecoverySystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RehabilitationController : ControllerBase
    {
        private readonly IRehabilitationService _rehabilitationService;

        public RehabilitationController(IRehabilitationService rehabilitationService)
        {
            _rehabilitationService = rehabilitationService;
        }

        // POST: /api/Rehabilitation/plan/{patientId}
        [HttpPost("plan/{patientId}")]
        public async Task<IActionResult> CreatePlan(int patientId, [FromBody] CreateRehabilitationPlanDto dto)
        {
            await _rehabilitationService.AddRehabilitationPlanAsync(patientId, dto);
            return Ok(new { Message = "Rehabilitation plan created successfully." });
        }

        // PUT: /api/Rehabilitation/progress/{id}
        [HttpPut("progress/{id}")]
        public async Task<IActionResult> UpdateProgress(int id, [FromBody] UpdateRehabilitationProgressDto dto)
        {
            var updated = await _rehabilitationService.UpdateRehabilitationProgressAsync(id, dto);
            if (!updated)
                return NotFound(new { Message = $"No rehabilitation record found with ID {id}." });

            return Ok(new { Message = "Progress updated successfully." });
        }

        // GET: /api/Rehabilitation/progress/{patientId}
        [HttpGet("progress/{patientId}")]
        public async Task<IActionResult> GetProgress(int patientId)
        {
            var result = await _rehabilitationService.GetRehabilitationProgressAsync(patientId);
            if (!result.Any())
                return NotFound(new { Message = $"No rehabilitation records found for patient ID {patientId}." });

            return Ok(result);
        }

        // PUT: /api/Rehabilitation/plan/{id}
        [HttpPut("plan/{id}")]
        public async Task<IActionResult> UpdatePlan(int id, [FromBody] UpdateRehabilitationPlanDto dto)
        {
            var updated = await _rehabilitationService.UpdateRehabilitationPlanAsync(id, dto);
            if (!updated)
                return NotFound(new { Message = $"No rehabilitation plan found with ID {id}." });

            return Ok(new { Message = "Plan updated successfully." });
        }

        // DELETE: /api/Rehabilitation/plan/{id}
        [HttpDelete("plan/{id}")]
        public async Task<IActionResult> DeletePlan(int id)
        {
            var deleted = await _rehabilitationService.DeleteRehabilitationPlanAsync(id);
            if (!deleted)
                return NotFound(new { Message = $"No rehabilitation plan found with ID {id}." });

            return Ok(new { Message = "Plan deleted successfully." });
        }
    }
}

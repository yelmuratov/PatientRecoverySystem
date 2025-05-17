using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MassTransit;
using PatientRecoverySystem.Application.Events;
using PatientRecoverySystem.Application.DTOs;
using PatientRecoverySystem.Application.Interfaces;

namespace PatientRecoverySystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RecoveryLogController : ControllerBase
    {
        private readonly IRecoveryLogService _recoveryLogService;
        private readonly IPublishEndpoint _publishEndpoint;

        public RecoveryLogController(IRecoveryLogService recoveryLogService, IPublishEndpoint publishEndpoint)
        {
            _recoveryLogService = recoveryLogService;
            _publishEndpoint = publishEndpoint; // ✅ save it
        }

        [HttpPost("{patientId}")]
        [Authorize(Roles = "Doctor,AdminDoctor,Moderator,Patient")]
        public async Task<IActionResult> AddRecoveryLog(int patientId, [FromBody] RecoveryLogDto logDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _recoveryLogService.AddRecoveryLogAsync(patientId, logDto);
            return Ok(new { message = "Recovery log added successfully" });
        }

        [HttpGet("patient/{patientId}")]
        [Authorize(Roles = "Doctor,AdminDoctor,Moderator,Patient")]
        public async Task<IActionResult> GetRecoveryLogsByPatientId(int patientId)
        {
            var logs = await _recoveryLogService.GetRecoveryLogsByPatientIdAsync(patientId);
            return Ok(logs);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Doctor,AdminDoctor,Moderator,Patient")]
        public async Task<IActionResult> GetRecoveryLogById(int id)
        {
            var log = await _recoveryLogService.GetRecoveryLogByIdAsync(id);
            if (log == null) return NotFound();
            return Ok(log);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Doctor,AdminDoctor,Moderator,Patient")]
        public async Task<IActionResult> UpdateRecoveryLog(int id, [FromBody] RecoveryLogDto logDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updatedLog = await _recoveryLogService.UpdateRecoveryLogAsync(id, logDto);
            if (updatedLog == null) return NotFound();

            return Ok(updatedLog);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Doctor,AdminDoctor,Moderator,Patient")]
        public async Task<IActionResult> DeleteRecoveryLog(int id)
        {
            var log = await _recoveryLogService.GetRecoveryLogByIdAsync(id);
            if (log == null) return NotFound();

            await _recoveryLogService.DeleteRecoveryLogAsync(id);
            return NoContent();
        }

        [HttpGet("all")]
        [Authorize(Roles = "AdminDoctor,Moderator")]
        public async Task<IActionResult> GetAllRecoveryLogs()
        {
            var logs = await _recoveryLogService.GetAllRecoveryLogsAsync();
            return Ok(logs);
        }

        [HttpGet("doctor/{doctorId}")]
        [Authorize(Roles = "AdminDoctor,Moderator")]
        public async Task<IActionResult> GetRecoveryLogsByDoctorId(int doctorId)
        {
            var logs = await _recoveryLogService.GetRecoveryLogsByDoctorIdAsync(doctorId);
            return Ok(logs);
        }
    }
}

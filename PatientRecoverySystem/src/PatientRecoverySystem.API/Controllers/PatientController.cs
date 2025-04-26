using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PatientRecoverySystem.Application.DTOs;
using PatientRecoverySystem.Application.Interfaces;
using PatientRecoverySystem.Domain.Interfaces;

namespace PatientRecoverySystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;
        private readonly IDoctorService _doctorService; // Assuming you have a doctor service to check doctor existence

        public PatientController(IPatientService patientService, IDoctorService doctorService)
        {
            _patientService = patientService;
            _doctorService = doctorService; // Injecting the doctor service to check for doctor existence
        }

        /// <summary>
        /// Get all patients
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Doctor,AdminDoctor,Moderator")]
        public async Task<IActionResult> GetAllPatients()
        {
            var patients = await _patientService.GetAllPatientsAsync(User);
            return Ok(patients);
        }

        /// <summary>
        /// Get a patient by ID
        /// </summary>
        [HttpGet("{id}")]
        [Authorize(Roles = "Doctor,AdminDoctor,Moderator")]
        public async Task<IActionResult> GetPatientById(int id)
        {
            var patient = await _patientService.GetPatientByIdAsync(id);
            if (patient == null) return NotFound();
            return Ok(patient);
        }

        /// <summary>
        /// Get all patients for a specific doctor
        /// </summary>
        [HttpGet("doctor/{doctorId}")]
        [Authorize(Roles = "AdminDoctor,Moderator")]
        public async Task<IActionResult> GetPatientsByDoctorId(int doctorId)
        {
            var doctorExists = await _doctorService.GetDoctorByIdAsync(doctorId);
            if (doctorExists == null)
            {
                return NotFound(new { message = "Doctor not found" });
            }

            var patients = await _patientService.GetPatientsByDoctorIdAsync(doctorId);
            return Ok(patients);
        }

        /// <summary>
        /// Create a new patient
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Doctor,AdminDoctor,Moderator")]
        public async Task<IActionResult> CreatePatient([FromBody] PatientDto patientDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var createdPatient = await _patientService.CreatePatientAsync(patientDto);
            return CreatedAtAction(nameof(GetPatientById), new { id = createdPatient.Id }, createdPatient);
        }

        /// <summary>
        /// Add a recovery log to a specific patient
        /// </summary>
        [HttpPost("{patientId}/recovery-log")]
        [Authorize(Roles = "Doctor,AdminDoctor,Moderator,Patient")]
        public async Task<IActionResult> AddRecoveryLog(int patientId, [FromBody] RecoveryLogDto logDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _patientService.AddRecoveryLogAsync(patientId, logDto);
            return Ok(new { message = "Recovery log added successfully" });
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PatientRecoverySystem.Application.DTOs;
using PatientRecoverySystem.Application.Interfaces;
using PatientRecoverySystem.Domain.Interfaces;
using PatientRecoverySystem.Application.Parameters; 
using PatientRecoverySystem.Domain.Entities; 
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
        /// Returns a paginated, searchable, sortable list of patients.
        /// </summary>
        /// <param name="parameters">Query parameters for pagination, filtering, and sorting.</param>
        /// <returns>Paginated list of patients</returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<PatientDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllPatients([FromQuery] PatientQueryParameters parameters)
        {
            var result = await _patientService.GetAllPatientsAsync(parameters, User);
            return Ok(result);
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
        [Authorize(Roles = "AdminDoctor,Moderator,Doctor")]
        public async Task<IActionResult> GetPatientsByDoctorId(int doctorId)
        {
            var doctorExists = await _doctorService.GetDoctorByIdAsync(doctorId, User);
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
        /// Update a patient by ID
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Doctor,AdminDoctor,Moderator")]
        public async Task<IActionResult> UpdatePatient(int id, [FromBody] PatientDto patientDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updatedPatient = await _patientService.UpdatePatientAsync(id, patientDto);
            if (updatedPatient == null) return NotFound();

            return Ok(updatedPatient);
        }

        /// <summary>
        /// Delete a patient by ID
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Doctor,AdminDoctor,Moderator")]
        public async Task<IActionResult> DeletePatient(int id)
        {
            var patient = await _patientService.GetPatientByIdAsync(id);
            if (patient == null) return NotFound();

            await _patientService.DeletePatientAsync(id);
            return NoContent();
        }
    }
}

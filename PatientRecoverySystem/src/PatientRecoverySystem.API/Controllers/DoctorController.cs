using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PatientRecoverySystem.Application.DTOs;
using PatientRecoverySystem.Application.Interfaces;
using PatientRecoverySystem.Application.Parameters;
using PatientRecoverySystem.Domain.Entities;

namespace PatientRecoverySystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // 🔒 Require Authentication for entire controller
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _doctorService;

        public DoctorController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        /// <summary>
        /// Get all doctors (Admin and normal doctors)
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "AdminDoctor,Moderator")]
        [ProducesResponseType(typeof(PagedResult<DoctorDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllDoctors([FromQuery] DoctorQueryParameters parameters)
        {
            var result = await _doctorService.GetAllDoctorsAsync(parameters, User);
            return Ok(result);
        }

        /// <summary>
        /// Get a doctor by ID
        /// </summary>
        [HttpGet("{id}")]
        [Authorize(Roles = "AdminDoctor,Moderator")]
        public async Task<IActionResult> GetDoctorById(int id)
        {
            try
            {
                var doctor = await _doctorService.GetDoctorByIdAsync(id, User);
                return Ok(doctor);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }


        /// <summary>
        /// Create a new doctor (Only AdminDoctor and Moderator can use this ideally)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "AdminDoctor,Moderator")] // 🔒 Only AdminDoctor and Moderator can create new doctors
        public async Task<IActionResult> CreateDoctor([FromBody] DoctorDto doctorDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var createdDoctor = await _doctorService.CreateDoctorAsync(doctorDto, User);
            return CreatedAtAction(nameof(GetDoctorById), new { id = createdDoctor.Id }, createdDoctor);
        }

        /// <summary>
        /// Update a doctor by ID (Only AdminDoctor and Moderator can use this ideally)
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "AdminDoctor,Moderator")]
        public async Task<IActionResult> UpdateDoctor(int id, [FromBody] DoctorDto doctorDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updated = await _doctorService.UpdateDoctorAsync(id, doctorDto, User); // pass User here
            if (updated == null) return NotFound();

            return Ok(updated);
        }


        /// <summary>
        /// Delete a doctor by ID (Only AdminDoctor and Moderator can use this ideally)
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "AdminDoctor,Moderator")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            await _doctorService.DeleteDoctorAsync(id, User);
            return NoContent();
        }
    }
}

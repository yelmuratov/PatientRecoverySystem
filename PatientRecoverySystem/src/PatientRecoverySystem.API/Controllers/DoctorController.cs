using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PatientRecoverySystem.Application.DTOs;
using PatientRecoverySystem.Application.Interfaces;

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
        [Authorize(Roles = "AdminDoctor,Moderator")] // 🔒 Only AdminDoctor and Doctor can access this endpoint
        public async Task<IActionResult> GetAllDoctors()
        {
            var doctors = await _doctorService.GetAllDoctorsAsync();
            return Ok(doctors);
        }

        /// <summary>
        /// Get a doctor by ID
        /// </summary>
        [HttpGet("{id}")]
        [Authorize(Roles = "AdminDoctor,Moderator")] // 🔒 Only AdminDoctor and Moderator can access this endpoint
        public async Task<IActionResult> GetDoctorById(int id)
        {
            var doctor = await _doctorService.GetDoctorByIdAsync(id);
            if (doctor == null) return NotFound();

            return Ok(doctor);
        }

        /// <summary>
        /// Create a new doctor (Only AdminDoctor and Moderator can use this ideally)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "AdminDoctor,Moderator")] // 🔒 Only AdminDoctor and Moderator can create new doctors
        public async Task<IActionResult> CreateDoctor([FromBody] DoctorDto doctorDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var createdDoctor = await _doctorService.CreateDoctorAsync(doctorDto);
            return CreatedAtAction(nameof(GetDoctorById), new { id = createdDoctor.Id }, createdDoctor);
        }
    }
}

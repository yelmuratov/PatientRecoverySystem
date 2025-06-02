using Microsoft.AspNetCore.Mvc;
using PatientRecoverySystem.Application.DTOs;
using PatientRecoverySystem.Application.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using PatientRecoverySystem.Domain.Interfaces;


namespace PatientRecoverySystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ITokenBlacklistService _tokenBlacklistService;

        public AuthController(IAuthService authService, ITokenBlacklistService tokenBlacklistService)
        {
            _authService = authService;
            _tokenBlacklistService = tokenBlacklistService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var token = await _authService.LoginAsync(loginDto);
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(new { message = "Invalid email or password." });
            }

            return Ok(new { token });
        }

        [HttpGet("me")]
        [Authorize]
        public IActionResult GetCurrentUser()
        {
            var id = User.FindFirstValue("id");
            var email = User.FindFirstValue(ClaimTypes.Email);
            var role = User.FindFirstValue(ClaimTypes.Role);
            var FullName = User.FindFirstValue("FullName");
            
            // if role is patient, return only id, email, role, FullName, phoneNumber
            if (role == "Patient")
            {
                var phoneNumber = User.FindFirstValue("Phone");
                var dateOfBirth = User.FindFirstValue("DateOfBirth");
                var Doctor = User.FindFirstValue("Doctor");
                

                return Ok(new
                {
                    id,
                    email,
                    role,
                    FullName,
                    phoneNumber,
                    dateOfBirth,
                    Doctor
                });
            }

            return Ok(new
            {
                id,
                email,
                role,
                FullName
            });
        }

        [HttpPost("logout")]
        [Authorize]
        public IActionResult Logout()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            _tokenBlacklistService.BlacklistToken(token);

            return Ok(new { message = "Logged out successfully. Token deactivated." });
        }

    }
}

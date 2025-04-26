using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PatientRecoverySystem.NotificationsService.Data;
using PatientRecoverySystem.NotificationsService.Models;

namespace PatientRecoverySystem.NotificationsService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationsController : ControllerBase
    {
        private readonly NotificationDbContext _context;

        public NotificationsController(NotificationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all notifications with pagination and filtering
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<NotificationLog>>> GetAllNotifications([FromQuery] NotificationQueryParameters parameters)
        {
            var query = _context.NotificationLogs.AsQueryable();

            // Apply Filtering
            if (!string.IsNullOrWhiteSpace(parameters.EmergencyType))
            {
                query = query.Where(n => n.EmergencyType.Contains(parameters.EmergencyType));
            }

            // Apply Pagination
            var notifications = await query
                .OrderByDescending(n => n.ReceivedAt)
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            return Ok(notifications);
        }
    }
}

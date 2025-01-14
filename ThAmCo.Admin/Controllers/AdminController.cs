using ThAmCo.Admin.Services;
using Microsoft.AspNetCore.Mvc;

namespace ThAmCo.Admin.Controllers
{
    [ApiController]
    [Route("api/admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _adminService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpPatch("orders/{id}/status")]
        public async Task<IActionResult> ChangeOrderStatus(int id, [FromBody] string status)
        {
            var success = await _adminService.ChangeOrderStatusAsync(id, status);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpDelete("users/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var success = await _adminService.DeleteUserAccountAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }

    }
}

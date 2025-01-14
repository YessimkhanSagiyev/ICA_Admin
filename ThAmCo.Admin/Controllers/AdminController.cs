using ThAmCo.Admin.Services;
using Microsoft.AspNetCore.Mvc;

namespace ThAmCo.Admin.Controllers
{
    [ApiController]
    [Route("api/admin")]
    public class AdminController : ControllerBase
    {
        private readonly CoreService _icaDevService;

    public AdminController(CoreService icaDevService)
    {
        _icaDevService = icaDevService;
    }

    [HttpPost("change-order-status")]
    public async Task<IActionResult> ChangeOrderStatusAsync([FromBody] ChangeStatusRequest request)
    {
        await _icaDevService.ChangeOrderStatusAsync(request.OrderId, request.NewStatus);
        return Ok("Order status updated by admin.");
    }

    [HttpDelete("delete-user/{userId}")]
    public async Task<IActionResult> DeleteUserAccountAsync(Guid userId)
    {
        await _icaDevService.DeleteUserAccountAsync(userId);
        return Ok("User account deleted by admin.");
    }

    }
}

public class Order
{
    public int Id { get; set; }
    public string Status { get; set; }
}

using ThAmCo.Admin.Models;

namespace ThAmCo.Admin.Services
{
    public interface IAdminService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<Order> MarkOrderAsDispatchedAsync(int orderId, DateTime dispatchedDate);
        Task<bool> DeleteUserAccountAsync(int userId);
        Task<bool> ChangeOrderStatusAsync(int orderId, string status);
    }
}

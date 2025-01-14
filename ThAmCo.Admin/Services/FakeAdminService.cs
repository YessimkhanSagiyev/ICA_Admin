using ThAmCo.Admin.Models;
using ThAmCo.Admin.Services;

namespace ThAmCo.Admin.Services
{
    public class FakeAdminService : IAdminService
    {
       private readonly List<User> _users;
        private readonly List<Order> _orders;

        public FakeAdminService()
        {
            _users = new List<User>
            {
                new User { Id = 1, Name = "John Doe", Email = "john.doe@example.com" },
                new User { Id = 2, Name = "Jane Smith", Email = "jane.smith@example.com" }
            };

            _orders = new List<Order>
            {
                new Order { Id = 1, OrderDate = DateTime.UtcNow.AddDays(-5), DispatchDate = null, Status = "Pending", UserId = 1 },
                new Order { Id = 2, OrderDate = DateTime.UtcNow.AddDays(-3), DispatchDate = DateTime.UtcNow.AddDays(-1), Status = "Shipped", UserId = 2 }
            };
        }

        public Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return Task.FromResult(_users.AsEnumerable());
        }

        public Task<Order> MarkOrderAsDispatchedAsync(int orderId, DateTime dispatchedDate)
        {
            var order = _orders.FirstOrDefault(o => o.Id == orderId);
            if (order != null)
            {
                order.DispatchDate = dispatchedDate;
                order.Status = "Dispatched";
            }
            return Task.FromResult(order);
        }

        public Task<bool> DeleteUserAccountAsync(int userId)
        {
            var user = _users.FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                _users.Remove(user);
                _orders.RemoveAll(o => o.UserId == userId);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task<bool> ChangeOrderStatusAsync(int orderId, string status)
        {
            var order = _orders.FirstOrDefault(o => o.Id == orderId);
            if (order != null)
            {
                order.Status = status;
            }
            return Task.FromResult(order != null);
        }
        public List<Order> GetAllOrders()
        {
            return _orders;
        }

    }
}

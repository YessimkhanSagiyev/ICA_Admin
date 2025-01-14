using ThAmCo.Admin.Data;
using ThAmCo.Admin.Models;
using ThAmCo.Admin.Services;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Net.Http.Json;

namespace ThAmCo.Admin.Services
{
    public class AdminService : IAdminService
    {
        private readonly AdminDbContext _dbContext;
        private readonly HttpClient _httpClient;

        public AdminService(AdminDbContext dbContext, HttpClient httpClient)
        {
            _dbContext = dbContext;
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _dbContext.Users.ToListAsync();
        }

        public async Task<Order> MarkOrderAsDispatchedAsync(int orderId, DateTime dispatchedDate)
        {
            var order = await _dbContext.Orders.FindAsync(orderId);
            if (order != null)
            {
                order.DispatchDate = dispatchedDate;
                _dbContext.Orders.Update(order);
                await _dbContext.SaveChangesAsync();
            }
            return order;
        }

        public async Task<bool> DeleteUserAccountAsync(int userId)
        {
            var response = await _httpClient.DeleteAsync($"/api/core/users/{userId}");
            if (response.IsSuccessStatusCode)
            {
                var user = await _dbContext.Users.FindAsync(userId);
                if (user != null)
                {
                    _dbContext.Users.Remove(user);
                    await _dbContext.SaveChangesAsync();
                }
                return true;
            }
            return false;
        }

        public async Task<bool> ChangeOrderStatusAsync(int orderId, string status)
        {
            var order = await _dbContext.Orders.FindAsync(orderId);
            if (order != null)
            {
                order.Status = status;
                _dbContext.Orders.Update(order);
                await _dbContext.SaveChangesAsync();

                var response = await _httpClient.PutAsJsonAsync($"/api/core/orders/{orderId}/status", new { status });
                return response.IsSuccessStatusCode;
            }
            return false;
        }
    }
}

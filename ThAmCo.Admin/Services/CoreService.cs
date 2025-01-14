using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

public class CoreService    
{   
    private readonly HttpClient _httpClient;

    public CoreService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task ChangeOrderStatusAsync(Guid orderId, string newStatus)
    {
        var request = new ChangeStatusRequest
        {
            OrderId = orderId,
            NewStatus = newStatus
        };

        var response = await _httpClient.PutAsJsonAsync("api/orders/change-status", request);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteUserAccountAsync(Guid userId)
    {
        var response = await _httpClient.DeleteAsync($"api/orders/delete-user/{userId}");
        response.EnsureSuccessStatusCode();
    }
}

// DTOs
public class ChangeStatusRequest
{
    public Guid OrderId { get; set; }
    public string NewStatus { get; set; }
}
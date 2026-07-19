using Microsoft.AspNetCore.SignalR;
using QrAccessSystem.API.Hubs;
using QrAccessSystem.Application.Interfaces;

namespace QrAccessSystem.API.Services;

public class RealTimeService : IRealTimeService
{
    private readonly IHubContext<AccessHub> _hubContext;

    public RealTimeService(IHubContext<AccessHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task SendAccessNotificationAsync(string message, bool isSuccess)
    {
        // Odaya (AccessHub) bağlı olan "Tüm" istemcilere "ReceiveAccessNotification" adında bir olay (event) fırlatıyoruz.
        await _hubContext.Clients.All.SendAsync("ReceiveAccessNotification", message, isSuccess);
    }
}
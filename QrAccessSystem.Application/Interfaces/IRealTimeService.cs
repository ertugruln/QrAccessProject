namespace QrAccessSystem.Application.Interfaces;

public interface IRealTimeService
{
    // Ekrana gönderilecek mesaj ve işlemin başarılı olup olmadığı bilgisi
    Task SendAccessNotificationAsync(string message, bool isSuccess);
}
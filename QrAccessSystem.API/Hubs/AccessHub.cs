using Microsoft.AspNetCore.SignalR;

namespace QrAccessSystem.API.Hubs;

// Bu sınıf boş kalabilir, çünkü istemciler (clients) buraya bir şey göndermeyecek, 
// sadece buraya bağlanıp bizim onlara fırlatacağımız logları dinleyecekler.
public class AccessHub : Hub
{
}
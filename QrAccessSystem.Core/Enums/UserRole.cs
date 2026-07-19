namespace QrAccessSystem.Core.Enums;

public enum UserRole
{
    Admin = 1,          // Tüm sisteme hakim yönetici
    SecurityGuard = 2,  // Sadece geçişleri izleyen ve ziyaretçi ekleyen güvenlik
    HumanResources = 3  // Personel ekleme çıkarma yapan İK
}
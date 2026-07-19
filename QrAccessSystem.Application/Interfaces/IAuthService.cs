using QrAccessSystem.Core.Entities;

namespace QrAccessSystem.Application.Interfaces;

public interface IAuthService
{
    // Yeni kullanıcı kaydolurken şifresini şifreler (Hash ve Salt üretir)
    void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
    
    // Giriş yaparken girilen şifrenin, veritabanındaki şifreyle eşleşip eşleşmediğini kontrol eder
    bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
    
    // Şifre doğruysa, kullanıcıya özel imzalı bir JWT (Token) üretir
    string CreateToken(AppUser user);
}
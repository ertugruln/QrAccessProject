using MediatR;
using QrAccessSystem.Application.Interfaces;
using QrAccessSystem.Application.Wrappers;
using QrAccessSystem.Core.Entities;

namespace QrAccessSystem.Application.Features.Auth.Commands;

public class LoginCommandHandler : IRequestHandler<LoginCommand, Response<string>>
{
    private readonly IGenericRepository<AppUser> _userRepository;
    private readonly IAuthService _authService;

    public LoginCommandHandler(IGenericRepository<AppUser> userRepository, IAuthService authService)
    {
        _userRepository = userRepository;
        _authService = authService;
    }

    public async Task<Response<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        // 1. Kullanıcı sistemde var mı?
        var users = await _userRepository.FindAsync(u => u.Username == request.Username);
        var user = users.FirstOrDefault();

        if (user == null || !user.IsActive)
            return Response<string>.Fail("Kullanıcı bulunamadı veya hesabınız pasife alınmış.");

        // 2. Girilen şifre doğru mu? (Veritabanındaki tuz ile tekrar hash'leyip karşılaştırıyoruz)
        if (!_authService.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            return Response<string>.Fail("Hatalı şifre.");

        // 3. Şifre doğruysa kriptografik bileti (Token) üret ve ver
        var token = _authService.CreateToken(user);

        return Response<string>.Success(token, "Giriş başarılı.");
    }
}
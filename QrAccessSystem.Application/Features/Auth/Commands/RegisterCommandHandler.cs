using MediatR;
using QrAccessSystem.Application.Interfaces;
using QrAccessSystem.Application.Wrappers;
using QrAccessSystem.Core.Entities;

namespace QrAccessSystem.Application.Features.Auth.Commands;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Response<Guid>>
{
    private readonly IGenericRepository<AppUser> _userRepository;
    private readonly IAuthService _authService;

    public RegisterCommandHandler(IGenericRepository<AppUser> userRepository, IAuthService authService)
    {
        _userRepository = userRepository;
        _authService = authService;
    }

    public async Task<Response<Guid>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        // 1. Aynı kullanıcı adı veya e-posta ile kayıtlı biri var mı kontrolü
        var existingUser = await _userRepository.FindAsync(u => u.Username == request.Username || u.Email == request.Email);
        if (existingUser.Any())
        {
            throw new Exceptions.ValidationException(new List<string> { "Bu kullanıcı adı veya e-posta adresi zaten kullanılıyor." });
        }

        // 2. Şifreyi açık metin (Plain Text) olarak değil, kriptolayarak (Hash) hazırlama
        _authService.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

        // 3. Kullanıcıyı oluşturup veritabanına kaydetme
        var user = new AppUser
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Username = request.Username,
            Email = request.Email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            Role = request.Role,
            IsActive = true
        };

        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();

        return Response<Guid>.Success(user.Id, "Kullanıcı başarıyla kaydedildi.");
    }
}
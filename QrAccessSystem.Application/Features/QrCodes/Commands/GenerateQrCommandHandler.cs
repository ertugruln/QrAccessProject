using MediatR;
using QrAccessSystem.Application.Interfaces;
using QrAccessSystem.Application.Wrappers;
using QrAccessSystem.Core.Entities;

namespace QrAccessSystem.Application.Features.QrCodes.Commands;

public class GenerateQrCommandHandler : IRequestHandler<GenerateQrCommand, Response<string>>
{
    private readonly IGenericRepository<Employee> _employeeRepository;
    private readonly IGenericRepository<QrCode> _qrCodeRepository;
    private readonly IQrCodeService _qrCodeService;

    public GenerateQrCommandHandler(
        IGenericRepository<Employee> employeeRepository,
        IGenericRepository<QrCode> qrCodeRepository,
        IQrCodeService qrCodeService)
    {
        _employeeRepository = employeeRepository;
        _qrCodeRepository = qrCodeRepository;
        _qrCodeService = qrCodeService;
    }

    public async Task<Response<string>> Handle(GenerateQrCommand request, CancellationToken cancellationToken)
    {
        // 1. Personel gerçekten var mı ve aktif mi kontrolü
        var employee = await _employeeRepository.GetByIdAsync(request.EmployeeId);
        if (employee == null || !employee.IsActive)
        {
            throw new Exceptions.ValidationException(new List<string> { "Aktif personel bulunamadı veya yetkisiz işlem." });
        }

        // 2. Kriptografik QR metnini üretme
        var uniqueToken = Guid.NewGuid().ToString("N");
        var securePayload = _qrCodeService.GenerateSecurePayload(request.EmployeeId, uniqueToken);
        
        // 3. Geçerlilik süresini belirleme (Şu anki zamandan 30 saniye sonrası)
        var expireDate = DateTime.UtcNow.AddSeconds(30);

        // 4. Üretilen QR'ı veritabanına kaydetme (Böylece okutulduğunda kontrol edebileceğiz)
        var qrCodeEntity = new QrCode
        {
            EmployeeId = request.EmployeeId,
            Payload = securePayload, // QR'ın içindeki o uzun şifreli metin
            ExpireDate = expireDate,
            IsRevoked = false
        };

        await _qrCodeRepository.AddAsync(qrCodeEntity);
        await _qrCodeRepository.SaveChangesAsync();

        // Personelin mobil uygulamasında ekrana basılacak olan şifreli metin döndürülür
        return Response<string>.Success(securePayload, "QR Kod başarıyla üretildi. 30 saniye boyunca geçerlidir.");
    }
}
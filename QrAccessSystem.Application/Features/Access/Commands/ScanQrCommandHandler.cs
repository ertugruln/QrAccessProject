using MediatR;
using QrAccessSystem.Application.Interfaces;
using QrAccessSystem.Application.Wrappers;
using QrAccessSystem.Core.Entities;

namespace QrAccessSystem.Application.Features.Access.Commands;

public class ScanQrCommandHandler : IRequestHandler<ScanQrCommand, Response<string>>
{
    private readonly IGenericRepository<QrCode> _qrCodeRepository;
    private readonly IGenericRepository<AccessLog> _accessLogRepository;
    private readonly IQrCodeService _qrCodeService;

    public ScanQrCommandHandler(
        IGenericRepository<QrCode> qrCodeRepository, 
        IGenericRepository<AccessLog> accessLogRepository,
        IQrCodeService qrCodeService)
    {
        _qrCodeRepository = qrCodeRepository;
        _accessLogRepository = accessLogRepository;
        _qrCodeService = qrCodeService;
    }

    public async Task<Response<string>> Handle(ScanQrCommand request, CancellationToken cancellationToken)
    {
        // 1. Kriptografik İmza Kontrolü (Sahte QR engelleme)
        if (!_qrCodeService.ValidateSignature(request.Payload))
        {
            return Response<string>.Fail("Geçersiz veya manipüle edilmiş QR kod tespit edildi!");
        }

        // 2. Veritabanından QR kodu bulma
        var qrCodes = await _qrCodeRepository.FindAsync(x => x.Payload == request.Payload);
        var qrCode = qrCodes.FirstOrDefault();

        if (qrCode == null)
            return Response<string>.Fail("Sistemde böyle bir QR kod kaydı bulunamadı.");

        // Log nesnemizi hazırlıyoruz
        var accessLog = new AccessLog
        {
            EmployeeId = qrCode.EmployeeId,
            QrCodeId = qrCode.Id,
            AccessType = request.AccessType,
            AccessTime = DateTime.UtcNow,
            IsSuccess = false
        };

        // 3. İptal Kontrolü
        if (qrCode.IsRevoked)
        {
            accessLog.FailureReason = "İptal edilmiş (Revoked) QR Kod.";
            await SaveLogAsync(accessLog);
            return Response<string>.Fail("Bu QR kod güvenlik sebebiyle iptal edilmiştir.");
        }

        // 4. Süre (Expiration) Kontrolü
        if (qrCode.ExpireDate < DateTime.UtcNow)
        {
            accessLog.FailureReason = "Süresi dolmuş QR Kod.";
            await SaveLogAsync(accessLog);
            return Response<string>.Fail("QR kodun süresi (30 saniye) dolmuş. Lütfen yeni bir QR üretin.");
        }

        // 5. Her şey başarılı! Log'u başarılı olarak güncelle.
        accessLog.IsSuccess = true;
        await SaveLogAsync(accessLog);

        string actionType = request.AccessType == Core.Enums.AccessType.Entry ? "Giriş" : "Çıkış";
        return Response<string>.Success("Geçiş Onaylandı", $"{actionType} işlemi başarıyla kaydedildi.");
    }

    // DRY Prensibi: Log kaydetme kodunu tekrar etmemek için küçük bir yardımcı metot
    private async Task SaveLogAsync(AccessLog log)
    {
        await _accessLogRepository.AddAsync(log);
        await _accessLogRepository.SaveChangesAsync();
    }
}
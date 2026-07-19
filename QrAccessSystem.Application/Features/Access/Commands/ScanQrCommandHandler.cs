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
    private readonly IRealTimeService _realTimeService; // YENİ EKLENDİ

    public ScanQrCommandHandler(
        IGenericRepository<QrCode> qrCodeRepository, 
        IGenericRepository<AccessLog> accessLogRepository,
        IQrCodeService qrCodeService,
        IRealTimeService realTimeService) // YENİ EKLENDİ
    {
        _qrCodeRepository = qrCodeRepository;
        _accessLogRepository = accessLogRepository;
        _qrCodeService = qrCodeService;
        _realTimeService = realTimeService;
    }

    public async Task<Response<string>> Handle(ScanQrCommand request, CancellationToken cancellationToken)
    {
        if (!_qrCodeService.ValidateSignature(request.Payload))
        {
            await _realTimeService.SendAccessNotificationAsync("Geçersiz veya manipüle edilmiş QR kod tespit edildi!", false);
            return Response<string>.Fail("Geçersiz veya manipüle edilmiş QR kod tespit edildi!");
        }

        var qrCodes = await _qrCodeRepository.FindAsync(x => x.Payload == request.Payload);
        var qrCode = qrCodes.FirstOrDefault();

        if (qrCode == null) return Response<string>.Fail("QR kod bulunamadı.");

        var accessLog = new AccessLog
        {
            EmployeeId = qrCode.EmployeeId,
            QrCodeId = qrCode.Id,
            AccessType = request.AccessType,
            AccessTime = DateTime.UtcNow,
            IsSuccess = false
        };

        if (qrCode.IsRevoked)
        {
            accessLog.FailureReason = "İptal edilmiş (Revoked) QR Kod.";
            await SaveLogAsync(accessLog);
            await _realTimeService.SendAccessNotificationAsync("Güvenlik İhlali: İptal edilmiş QR ile giriş denemesi!", false);
            return Response<string>.Fail("Bu QR kod iptal edilmiştir.");
        }

        if (qrCode.ExpireDate < DateTime.UtcNow)
        {
            accessLog.FailureReason = "Süresi dolmuş QR Kod.";
            await SaveLogAsync(accessLog);
            await _realTimeService.SendAccessNotificationAsync("Başarısız: QR kodun süresi dolmuş.", false);
            return Response<string>.Fail("QR kodun süresi dolmuş.");
        }

        accessLog.IsSuccess = true;
        await SaveLogAsync(accessLog);

        string actionType = request.AccessType == Core.Enums.AccessType.Entry ? "Giriş" : "Çıkış";
        
        // CANLI BİLDİRİM FIRLATIYORUZ!
        await _realTimeService.SendAccessNotificationAsync($"Geçiş Onaylandı: Personel {actionType} yaptı.", true);

        return Response<string>.Success("Geçiş Onaylandı", $"{actionType} işlemi başarıyla kaydedildi.");
    }

    private async Task SaveLogAsync(AccessLog log)
    {
        await _accessLogRepository.AddAsync(log);
        await _accessLogRepository.SaveChangesAsync();
    }
}
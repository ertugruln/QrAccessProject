using FluentValidation;
using QrAccessSystem.Application.Features.Visitors.Commands;

namespace QrAccessSystem.Application.Features.Visitors.Validations;

public class CreateVisitorCommandValidator : AbstractValidator<CreateVisitorCommand>
{
    public CreateVisitorCommandValidator()
    {
        RuleFor(p => p.FirstName).NotEmpty().WithMessage("Ad boş bırakılamaz.");
        RuleFor(p => p.LastName).NotEmpty().WithMessage("Soyad boş bırakılamaz.");
        RuleFor(p => p.IdentityNumber).NotEmpty().WithMessage("Kimlik numarası zorunludur.");
        RuleFor(p => p.HostEmployeeId).NotEmpty().WithMessage("Ev sahibi personel seçilmelidir.");

        // Tarih Validasyonları
        RuleFor(p => p.ExpectedArrival)
            .NotEmpty().WithMessage("Geliş saati zorunludur.")
            .GreaterThan(DateTime.UtcNow.AddMinutes(-5)).WithMessage("Geliş saati geçmiş bir zaman olamaz.");

        RuleFor(p => p.ExpectedDeparture)
            .NotEmpty().WithMessage("Çıkış saati zorunludur.")
            .GreaterThan(p => p.ExpectedArrival).WithMessage("Çıkış saati, giriş saatinden sonra olmalıdır.");
    }
}
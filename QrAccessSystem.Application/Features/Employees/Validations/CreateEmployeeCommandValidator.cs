using FluentValidation;
using QrAccessSystem.Application.Features.Employees.Commands;

namespace QrAccessSystem.Application.Features.Employees.Validations;

public class CreateEmployeeCommandValidator : AbstractValidator<CreateEmployeeCommand>
{
    public CreateEmployeeCommandValidator()
    {
        RuleFor(p => p.FirstName).NotEmpty().WithMessage("Ad alanı boş geçilemez.");
        RuleFor(p => p.LastName).NotEmpty().WithMessage("Soyad alanı boş geçilemez.");
        
        RuleFor(p => p.Email)
            .NotEmpty().WithMessage("E-posta alanı boş geçilemez.")
            .EmailAddress().WithMessage("Lütfen geçerli bir e-posta adresi giriniz.");

        RuleFor(p => p.Phone)
            .NotEmpty().WithMessage("Telefon numarası boş geçilemez.")
            .MinimumLength(10).WithMessage("Telefon numarası çok kısa.");

        RuleFor(p => p.DepartmentId)
            .NotEmpty().WithMessage("Personel bir departmana atanmalıdır.");
    }
}
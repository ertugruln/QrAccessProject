using FluentValidation;
using QrAccessSystem.Application.Features.Departments.Commands;

namespace QrAccessSystem.Application.Features.Departments.Validations;

public class CreateDepartmentCommandValidator : AbstractValidator<CreateDepartmentCommand>
{
    public CreateDepartmentCommandValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("Departman adı boş bırakılamaz.")
            .MaximumLength(50).WithMessage("Departman adı en fazla 50 karakter olabilir.");
            
        // İstersek Description için de kural ekleyebiliriz
        RuleFor(p => p.Description)
            .MaximumLength(200).WithMessage("Açıklama en fazla 200 karakter olabilir.");
    }
}
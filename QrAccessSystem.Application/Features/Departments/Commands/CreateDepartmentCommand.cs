using MediatR;
using AutoMapper;
using QrAccessSystem.Application.Interfaces;
using QrAccessSystem.Application.Wrappers;
using QrAccessSystem.Core.Entities;

namespace QrAccessSystem.Application.Features.Departments.Commands;

// 1. Dışarıdan Alacağımız Veri (Command)
public class CreateDepartmentCommand : IRequest<Response<Guid>>
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}

// 2. İşlemi Yapan Sınıf (Handler)
public class CreateDepartmentCommandHandler : IRequestHandler<CreateDepartmentCommand, Response<Guid>>
{
    private readonly IGenericRepository<Department> _repository;
    private readonly IMapper _mapper;

    public CreateDepartmentCommandHandler(IGenericRepository<Department> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Response<Guid>> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
    {
        // Gelen Command'i Entity'ye (Department) çeviriyoruz
        var department = _mapper.Map<Department>(request);

        await _repository.AddAsync(department);
        await _repository.SaveChangesAsync();

        return Response<Guid>.Success(department.Id, "Departman başarıyla oluşturuldu.");
    }
}
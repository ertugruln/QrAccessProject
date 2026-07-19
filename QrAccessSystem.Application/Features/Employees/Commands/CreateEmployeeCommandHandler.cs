using AutoMapper;
using MediatR;
using QrAccessSystem.Application.Interfaces;
using QrAccessSystem.Application.Wrappers;
using QrAccessSystem.Core.Entities;

namespace QrAccessSystem.Application.Features.Employees.Commands;

public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, Response<Guid>>
{
    private readonly IGenericRepository<Employee> _employeeRepository;
    private readonly IGenericRepository<Department> _departmentRepository;
    private readonly IMapper _mapper;

    public CreateEmployeeCommandHandler(
        IGenericRepository<Employee> employeeRepository, 
        IGenericRepository<Department> departmentRepository,
        IMapper mapper)
    {
        _employeeRepository = employeeRepository;
        _departmentRepository = departmentRepository;
        _mapper = mapper;
    }

    public async Task<Response<Guid>> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        // 1. İş Kuralı: Gönderilen DepartmentId gerçekten veritabanında var mı?
        var departmentExists = await _departmentRepository.GetByIdAsync(request.DepartmentId);
        if (departmentExists == null)
        {
            // Eğer departman yoksa, işlemi durdur ve hata dön.
            throw new Exceptions.ValidationException(new List<string> { "Seçilen departman sistemde bulunamadı." });
        }

        // 2. Mapping ve Kayıt İşlemi
        var employee = _mapper.Map<Employee>(request);
        employee.IsActive = true; // Yeni eklenen personel varsayılan olarak aktiftir

        await _employeeRepository.AddAsync(employee);
        await _employeeRepository.SaveChangesAsync();

        return Response<Guid>.Success(employee.Id, "Personel başarıyla sisteme eklendi.");
    }
}
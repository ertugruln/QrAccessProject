using AutoMapper;
using MediatR;
using QrAccessSystem.Application.Interfaces;
using QrAccessSystem.Application.Wrappers;
using QrAccessSystem.Core.Entities;

namespace QrAccessSystem.Application.Features.Visitors.Commands;

public class CreateVisitorCommandHandler : IRequestHandler<CreateVisitorCommand, Response<Guid>>
{
    private readonly IGenericRepository<Visitor> _visitorRepository;
    private readonly IGenericRepository<Employee> _employeeRepository;
    private readonly IMapper _mapper;

    public CreateVisitorCommandHandler(
        IGenericRepository<Visitor> visitorRepository,
        IGenericRepository<Employee> employeeRepository,
        IMapper mapper)
    {
        _visitorRepository = visitorRepository;
        _employeeRepository = employeeRepository;
        _mapper = mapper;
    }

    public async Task<Response<Guid>> Handle(CreateVisitorCommand request, CancellationToken cancellationToken)
    {
        // 1. İş Kuralı: Ev sahibi personel sistemde var mı ve aktif mi?
        var hostEmployee = await _employeeRepository.GetByIdAsync(request.HostEmployeeId);
        if (hostEmployee == null || !hostEmployee.IsActive)
        {
            throw new Exceptions.ValidationException(new List<string> { "Ziyaret edilmek istenen personel bulunamadı veya pasif durumda." });
        }

        // 2. Mapping: Gelen veriyi Visitor nesnesine çeviriyoruz.
        var visitor = _mapper.Map<Visitor>(request);
        
        // Ziyaretçi ilk eklendiğinde onaysızdır. (Güvenlik veya personel sonradan onaylayacak)
        visitor.IsApproved = false; 

        await _visitorRepository.AddAsync(visitor);
        await _visitorRepository.SaveChangesAsync();

        return Response<Guid>.Success(visitor.Id, "Ziyaretçi kaydı başarıyla oluşturuldu. Onay bekleniyor.");
    }
}
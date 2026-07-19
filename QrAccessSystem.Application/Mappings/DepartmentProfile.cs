using AutoMapper;
using QrAccessSystem.Application.Features.Departments.Commands;
using QrAccessSystem.Core.Entities;

namespace QrAccessSystem.Application.Mappings;

public class DepartmentProfile : Profile
{
    public DepartmentProfile()
    {
        // Command'den Entity'ye dönüşüm kuralı
        CreateMap<CreateDepartmentCommand, Department>();
    }
}
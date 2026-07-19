using AutoMapper;
using QrAccessSystem.Application.Features.Employees.Commands;
using QrAccessSystem.Core.Entities;

namespace QrAccessSystem.Application.Mappings;

public class EmployeeProfile : Profile
{
    public EmployeeProfile()
    {
        CreateMap<CreateEmployeeCommand, Employee>();
    }
}
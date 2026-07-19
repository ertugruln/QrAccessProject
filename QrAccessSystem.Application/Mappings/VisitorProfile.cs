using AutoMapper;
using QrAccessSystem.Application.Features.Visitors.Commands;
using QrAccessSystem.Core.Entities;

namespace QrAccessSystem.Application.Mappings;

public class VisitorProfile : Profile
{
    public VisitorProfile()
    {
        CreateMap<CreateVisitorCommand, Visitor>();
    }
}
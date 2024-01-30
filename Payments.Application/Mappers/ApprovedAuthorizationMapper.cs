using AutoMapper;
using Payments.Application.Dtos;
using Payments.Domain.Entities;

namespace Payments.Application.Mappers
{
    public class ApprovedAuthorizationMapper : Profile
    {
        public ApprovedAuthorizationMapper()
        {
            CreateMap<ApprovedAuthorizationDto, ApprovedAuthorization>().ReverseMap();
            CreateMap<Authorization, ApprovedAuthorization>().ReverseMap();
        }
    }
}


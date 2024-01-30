using AutoMapper;
using Payments.Application.Dtos;
using Payments.Application.Models;
using Payments.Domain.Entities;

namespace Payments.Application.Mappers
{
    public class AuthorizationMapper : Profile
    {
        public AuthorizationMapper()
        {
            CreateMap<AuthorizationRequestDto, Authorization>().ReverseMap();
            CreateMap<AuthorizationDto, Authorization>().ReverseMap();
            CreateMap<ExternalRequest, Authorization>().ReverseMap();
        }
    }
}

using AutoMapper;
using Contacts37.Domain.Entities;

namespace Contacts37.Application.Usecases.Contacts.Queries.GetByDdd
{
    public sealed class GetContactsByDddMapper : Profile
    {
        public GetContactsByDddMapper()
        {
            CreateMap<Contact, GetContactsByDddResponse>()
                .ForMember(dest => dest.DDDCode,
                    opt => opt.MapFrom(src => src.Region.DddCode));
        }
    }
}

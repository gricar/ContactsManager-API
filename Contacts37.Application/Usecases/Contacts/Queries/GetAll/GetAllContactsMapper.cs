using AutoMapper;
using Contacts37.Domain.Entities;

namespace Contacts37.Application.Usecases.Contacts.Queries.GetAll
{
    public sealed class GetAllContactsMapper : Profile
    {
        public GetAllContactsMapper()
        {
            CreateMap<Contact, GetAllContactsResponse>()
                .ForMember(dest => dest.DDDCode,
                    opt => opt.MapFrom(src => src.Region.DddCode));
        }
    }
}

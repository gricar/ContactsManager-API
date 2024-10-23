using AutoMapper;
using Contacts37.Domain.Entities;

namespace Contacts37.Application.Usecases.Contacts.Queries.GetAll
{
    public sealed class GetAllContactsMapper : Profile
    {
        public GetAllContactsMapper()
        {
            CreateMap<Contact, GetAllContactsResponse>();
        }
    }
}

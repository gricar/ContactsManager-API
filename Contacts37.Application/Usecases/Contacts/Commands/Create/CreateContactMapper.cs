using AutoMapper;
using Contacts37.Domain.Entities;

namespace Contacts37.Application.Usecases.Contacts.Commands.Create
{
    public sealed class CreateContactMapper : Profile
    {
        public CreateContactMapper()
        {
            CreateMap<Contact, CreateContactCommandResponse>();
        }
    }
}

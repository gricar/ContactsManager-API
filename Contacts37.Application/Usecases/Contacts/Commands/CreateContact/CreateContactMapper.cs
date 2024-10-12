using AutoMapper;
using Contacts37.Domain.Entities;

namespace Contacts37.Application.Usecases.Contacts.Commands.CreateContact
{
    public sealed class CreateContactMapper : Profile
    {
        public CreateContactMapper()
        {
            CreateMap<CreateContactCommand, Contact>();
            CreateMap<Contact, CreateContactCommandResponse>();
        }
    }
}

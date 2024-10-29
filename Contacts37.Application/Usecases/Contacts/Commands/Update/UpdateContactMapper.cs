using AutoMapper;
using Contacts37.Domain.Entities;

namespace Contacts37.Application.Usecases.Contacts.Commands.Update
{
    public class UpdateContactMapper : Profile
    {
        public UpdateContactMapper()
        {
            CreateMap<UpdateContactCommand, Contact>();
        }
    }
}

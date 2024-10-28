using AutoMapper;
using Contacts37.Domain.Entities;


namespace Contacts37.Application.Usecases.Contacts.Commands.Update
{
    public class UpdateContactCommand : Profile
    {
        public UpdateContactCommand()
        {
            CreateMap<UpdateContactCommand, Contact>();
        }
    }
}

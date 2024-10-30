using AutoMapper;
using Contacts37.Domain.Entities;

namespace Contacts37.Application.Usecases.Contacts.Commands.Delete
{
    public sealed class DeleteContactMapper : Profile
    {
        public DeleteContactMapper()
        {
            CreateMap<DeleteContactCommand, Contact>();
        }
    }
}

using AutoMapper;
using Contacts37.Domain.Entities;
using Contacts37.Domain.ValueObjects;

namespace Contacts37.Application.Usecases.Contacts.Commands.Update
{
    public class UpdateContactMapper : Profile
    {
        public UpdateContactMapper()
        {
            CreateMap<UpdateContactCommand, Contact>()
                .ForMember(dest => dest.Region,
                    opt => opt.MapFrom(src => Region.Create(src.DDDCode)));
        }
    }
}

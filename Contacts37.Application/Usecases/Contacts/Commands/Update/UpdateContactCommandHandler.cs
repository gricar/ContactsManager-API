using AutoMapper;
using Contacts37.Application.Contracts.Persistence;
using Contacts37.Domain.Entities;
using MediatR;

// classe responsável por atualizar as informações de usuário
namespace Contacts37.Application.Usecases.Contacts.Commands.Update;

    public class CreateContactCommandHandler : IRequestHandler<CreateContactCommand, CreateContactCommandResponse>
    {
        private readonly IContactRepository _contactRepository; // acessa os dados de usuário
        private readonly IMapper _mapper;                       

        public UpdateContactHandler(IContactRepository contactRepository, IMapper mapper) // construtor da classe
    {
        _contactRepository = contactRepository;
        _mapper = mapper;
    }

        public async Task HandleAsync(UpdateContactCommand command)
        {
        var existingUser = await _userRepository.GetAsync(command.Id);
        if (existingUser == null)
        {
            throw new Exception("Usuário não encontrado.");
        }

    //        string newTelephone = command.DddCode + command.Phone;
        if (await _userRepository.IsDddAndPhoneUniqueAsync(command.DddCode, command.Phone))
        {
            throw new Exception("A combinação de código de área e telefone já existe.");
        }

        //var updatedUser = _mapper.Map<Contact>(command);
        _mapper.Map(command, existingUser)

    //        existingUser.Name = command.Name;
    //        existingUser.Phone = command.Phone;
    //        existingUser.DddCode = command.DddCode;
    //        existingUser.Email = command.Email;

        await _userRepository.UpdateAsync(updatedUser);
        }
    }
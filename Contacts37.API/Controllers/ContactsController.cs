using MediatR;
using static Microsoft.AspNetCore.Http.StatusCodes;
using Microsoft.AspNetCore.Mvc;
using Contacts37.Application.Usecases.Contacts.Commands.Create;

namespace Contacts37.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly IMediator _dispatcher;

        public ContactsController(IMediator dispatcher)
        {
            _dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
        }

        [HttpPost]
        [ProducesResponseType(typeof(Guid), Status201Created)]
        [ProducesResponseType(Status400BadRequest)]
        public async Task<ActionResult<Guid>> PostContact([FromBody] CreateContactCommand command)
        {
            var response = await _dispatcher.Send(command);
            return CreatedAtAction(nameof(PostContact), new { id = response.Id }, response.Id);
        }


        // Método PUT para alterar os dados de um contato.
        [HttpPut("{id}")]
        // UpdateUserCommand deve conter os dados necessários para atualização
        public async Task<ActionResult<Guid>> UpdateUser(int id, [FromBody] UpdateUserCommand command)
        {
            // Verificação de ID
            if (id != command.Id)
            {
                return BadRequest("ID incompatível.");
            }
            // Tratamento do comando de Atualização
            try
            {
                //HandleAsync executa a lógica de atualização
                await _updateContactHandler.HandleAsync(command);
                // Código 204, operação executada mas sem retorno de conteúdo
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

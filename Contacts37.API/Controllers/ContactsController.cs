using MediatR;
using static Microsoft.AspNetCore.Http.StatusCodes;
using Microsoft.AspNetCore.Mvc;
using Contacts37.Application.Usecases.Contacts.Commands.Create;
using Contacts37.Application.Usecases.Contacts.Queries.GetAll;
using Contacts37.Application.Usecases.Contacts.Commands.Delete;

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
        [ProducesResponseType(typeof(CreateContactCommandResponse), Status201Created)]
        [ProducesResponseType(Status400BadRequest)]
        public async Task<ActionResult<Guid>> PostContact([FromBody] CreateContactCommand command)
        {
            var response = await _dispatcher.Send(command);
            return CreatedAtAction(nameof(PostContact), new { id = response.Id }, response.Id);
        }

		[HttpDelete("{id:guid}")]
		[ProducesResponseType(Status204NoContent)]
        [ProducesResponseType(Status400BadRequest)]
        public async Task<ActionResult<Guid>> DeleteContact([FromRoute] Guid id)
        {
            var response = await _dispatcher.Send(new DeleteContactCommand(id));
            return NoContent();
        }

        [HttpGet]
        [ProducesResponseType(typeof(GetAllContactsResponse), Status200OK)]
        [ProducesResponseType(Status400BadRequest)]
        public async Task<ActionResult<GetAllContactsResponse>> ListAllContacts()
        {
            var response = await _dispatcher.Send(new GetAllContactsRequest());
            return Ok(response);
        }
    }
}

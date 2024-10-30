using MediatR;
using static Microsoft.AspNetCore.Http.StatusCodes;
using Microsoft.AspNetCore.Mvc;
using Contacts37.Application.Usecases.Contacts.Commands.Create;
using Contacts37.Application.Usecases.Contacts.Commands.Update;

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

        [HttpGet]
        [ProducesResponseType(typeof(GetAllContactsResponse), Status200OK)]
        [ProducesResponseType(Status400BadRequest)]
        public async Task<ActionResult<GetAllContactsResponse>> ListAllContacts()
        {
            var response = await _dispatcher.Send(new GetAllContactsRequest());
            return Ok(response);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(Status204NoContent)]
        [ProducesResponseType(Status400BadRequest)]
        [ProducesResponseType(Status404NotFound)]
        public async Task<ActionResult<Unit>> UpdateUser([FromRoute] Guid id, [FromBody] UpdateContactCommand command)
        {
            var response = await _dispatcher.Send(command);
            return NoContent();
        }
    }
}
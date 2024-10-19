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
        public async Task<ActionResult<Guid>> PostConctact([FromBody] CreateContactCommand command)
        {
            var response = await _dispatcher.Send(command);
            return Ok(response);
        }
    }
}

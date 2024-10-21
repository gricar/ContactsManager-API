using MediatR;
using static Microsoft.AspNetCore.Http.StatusCodes;
using Microsoft.AspNetCore.Mvc;
using Contacts37.Application.Usecases.Contacts.Commands.CreateContact;
using Microsoft.EntityFrameworkCore;
using Contacts37.Domain.Entities;
using Contact37.Persistence;

namespace Contacts37.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly IMediator _dispatcher;
        private readonly ApplicationDbContext _context;

        public ContactsController(IMediator dispatcher, ApplicationDbContext context)
        {
            _dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [HttpPost]
        [ProducesResponseType(typeof(Guid), Status201Created)]
        public async Task<ActionResult<Guid>> PostConctact([FromBody] CreateContactCommand request)
        {
            var response = await _dispatcher.Send(request);
            return Ok(response);
        }

        // Método GET para retornar todos os contatos
        [HttpGet]
        public ActionResult<IEnumerable<Contact>> GetUsers()
        {
            var contacts = _context.Contacts.ToList();
            return Ok(contacts);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contacts37.Application.Responses
{
	public class CreateCommandResponse: BaseCommandResponse
	{
        public Guid Id { get; set; }
    }
}

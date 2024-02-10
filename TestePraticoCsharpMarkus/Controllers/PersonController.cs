using Application.Boudary.Person;
using Application.Command.Person;
using Infrastructure.Message.Interface;
using Microsoft.AspNetCore.Mvc;

namespace TestePraticoCsharpMarkus.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly ILogger<PersonController> _logger;
        private readonly IMessagesHandler _messagesHandler;

        public PersonController(ILogger<PersonController> logger,
                                IMessagesHandler messagesHandler)
        {
            _logger = logger;
            _messagesHandler = messagesHandler;
        }

        [HttpPost("")]
        public async Task<IActionResult> Person(CreatePersonInput input)
        {
            var response = await _messagesHandler.SendCommandAsync<CreatePersonCommand, CreatePersonOutput>
                     (
                        new CreatePersonCommand { Input = input }
                     ).ConfigureAwait(false);
            return Ok(response);
        }

        [HttpPut("")]
        public async Task<IActionResult> Put(UpdatePersonInput input)
        {
            var response = await _messagesHandler.SendCommandAsync<UpdatePersonCommand, UpdatePersonOutput>
                     (
                        new UpdatePersonCommand { Input = input }
                     ).ConfigureAwait(false);
            return Ok(response);
        }

        [HttpDelete("")]
        public async Task<IActionResult> Delete(int id)
        {
            var input = new DeletePersonInput
            {
                Id = id
            };
            var response = await _messagesHandler.SendCommandAsync<DeletePersonCommand, DeletePersonOutput>
                     (
                        new DeletePersonCommand { Input = input }
                     ).ConfigureAwait(false);
            return Ok(response);
        }

        [HttpGet("")]
        public async Task<IActionResult> Get([FromQuery] GetPersonsInput input)
        {
            var response = await _messagesHandler.SendCommandAsync<GetPersonsCommand, GetPersonsOutput>
                     (
                        new GetPersonsCommand { Input = input }
                     ).ConfigureAwait(false);
            return Ok(response);
        }

        [HttpGet("/{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var input = new GetPersonInput
            {
                Id = id
            };
            var response = await _messagesHandler.SendCommandAsync<GetPersonCommand, GetPersonOutput>
                     (
                        new GetPersonCommand { Input = input }
                     ).ConfigureAwait(false);
            return Ok(response);
        }
    }
}

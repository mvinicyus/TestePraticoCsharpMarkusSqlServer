using Application.Boudary.Person;
using Application.Command.Person;
using Domain.Entity.Person;
using Infrastructure.Message;
using Infrastructure.Message.Interface;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Repository;

namespace Application.Handler.Person
{
    public class CreatePersonHandler : IRequestHandler<CreatePersonCommand, CreatePersonOutput>
    {
        private readonly IMessagesHandler _messagesHandler;
        private readonly GenericRepository<PersonEntity, int> _PersonRepository;
        private readonly IHttpContextAccessor _accessor;

        public CreatePersonHandler(IMessagesHandler messagesHandler,
                           GenericRepository<PersonEntity, int> PersonRepository,
                           IHttpContextAccessor accessor)
        {
            _messagesHandler = messagesHandler;
            _PersonRepository = PersonRepository;
            _accessor = accessor;
        }

        public async Task<CreatePersonOutput> Handle(CreatePersonCommand command, CancellationToken cancellationToken)
        {
            if (command.IsValid())
            {
                await _PersonRepository
                       .BeginTransactionAsync(false)
                       .ConfigureAwait(false);

                var personExists = await _PersonRepository
                    .DbSet
                    .FirstOrDefaultAsync(x => x.Cpf.Trim() == command.Input.Cpf.Trim())
                    .ConfigureAwait(false); ;

                if (personExists != null)
                {
                    _ = ApplyErrorAsync("Pessoa ja foi cadastrada.");
                    return null;
                }

                var newPerson = new PersonEntity
                {
                    FullName = command.Input.FullName,
                    BirthDate = command.Input.BirthDate,
                    IncomeValue = command.Input.IncomeValue,
                    Cpf = command.Input.Cpf,
                    CreateDate = DateTime.UtcNow
                };

                _PersonRepository.DbSet.Add(newPerson);

                await _PersonRepository.CommitTransactionAsync();

                return new CreatePersonOutput();
            }

            Parallel.ForEach(command.ValidationResult.Errors, async error =>
            {
                await ApplyErrorAsync(error.ErrorMessage, command.MessageType).ConfigureAwait(false);
            });

            return null;
        }

        private async Task<bool> ApplyErrorAsync(string message, string messageType = "error")
        {
            var isError = true;
            await _messagesHandler.SendDomainNotificationAsync
            (
                new DomainNotification
                (
                    messageType,
                    message,
                    isError
                )
            ).ConfigureAwait(false);
            return false;
        }
    }
}

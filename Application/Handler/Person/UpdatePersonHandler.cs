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
    public class UpdatePersonHandler : IRequestHandler<UpdatePersonCommand, UpdatePersonOutput>
    {
        private readonly IMessagesHandler _messagesHandler;
        private readonly GenericRepository<PersonEntity, int> _PersonRepository;
        private readonly IHttpContextAccessor _accessor;

        public UpdatePersonHandler(IMessagesHandler messagesHandler,
                           GenericRepository<PersonEntity, int> PersonRepository,
                           IHttpContextAccessor accessor)
        {
            _messagesHandler = messagesHandler;
            _PersonRepository = PersonRepository;
            _accessor = accessor;
        }

        public async Task<UpdatePersonOutput> Handle(UpdatePersonCommand command, CancellationToken cancellationToken)
        {
            if (command.IsValid())
            {
                await _PersonRepository
                       .BeginTransactionAsync(false)
                       .ConfigureAwait(false);

                var personExists = await _PersonRepository
                    .DbSet
                    .FirstOrDefaultAsync(Person => Person.Id == command.Input.Id)
                    .ConfigureAwait(false);

                if (personExists == null)
                {
                    _ = ApplyErrorAsync("Pessoa não encontrado.");
                    return null;
                }

                personExists.FullName = command.Input.FullName;
                personExists.BirthDate = DateTime.Parse(command.Input.BirthDate);
                personExists.IncomeValue = decimal.Parse(command.Input.IncomeValue);
                personExists.Cpf = command.Input.Cpf;
                personExists.UpdateDate = DateTime.UtcNow;

                await _PersonRepository.CommitTransactionAsync();

                return new UpdatePersonOutput();
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

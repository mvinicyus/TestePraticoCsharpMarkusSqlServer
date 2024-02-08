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
    public class DeletePersonHandler : IRequestHandler<DeletePersonCommand, DeletePersonOutput>
    {
        private readonly IMessagesHandler _messagesHandler;
        private readonly GenericRepository<PersonEntity, int> _PersonRepository;
        private readonly IHttpContextAccessor _accessor;

        public DeletePersonHandler(IMessagesHandler messagesHandler,
                           GenericRepository<PersonEntity, int> PersonRepository,
                           IHttpContextAccessor accessor)
        {
            _messagesHandler = messagesHandler;
            _PersonRepository = PersonRepository;
            _accessor = accessor;
        }

        public async Task<DeletePersonOutput> Handle(DeletePersonCommand command, CancellationToken cancellationToken)
        {
            if (command.IsValid())
            {
                await _PersonRepository
                       .BeginTransactionAsync(false)
                       .ConfigureAwait(false);

                var Person = await _PersonRepository
                    .DbSet
                    .FirstOrDefaultAsync(Person => Person.Id == command.Input.Id)
                    .ConfigureAwait(false);

                if (Person == null)
                {
                    _ = ApplyErrorAsync("Pessoa não encontrada.");
                    return null;
                }

                _PersonRepository.DbSet.Remove(Person);

                await _PersonRepository.CommitTransactionAsync();

                return new DeletePersonOutput();
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

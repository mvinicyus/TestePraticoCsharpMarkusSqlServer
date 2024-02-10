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
    public class GetPersonHandler : IRequestHandler<GetPersonCommand, GetPersonOutput>
    {
        private readonly IMessagesHandler _messagesHandler;
        private readonly GenericRepository<PersonEntity, int> _PersonRepository;
        private readonly IHttpContextAccessor _accessor;

        public GetPersonHandler(IMessagesHandler messagesHandler,
                           GenericRepository<PersonEntity, int> PersonRepository,
                           IHttpContextAccessor accessor)
        {
            _messagesHandler = messagesHandler;
            _PersonRepository = PersonRepository;
            _accessor = accessor;
        }

        public async Task<GetPersonOutput> Handle(GetPersonCommand command, CancellationToken cancellationToken)
        {
            if (command.IsValid())
            {
                await _PersonRepository
                       .BeginTransactionAsync(true)
                       .ConfigureAwait(false);

                var person = await _PersonRepository
                    .DbSet
                    .FirstOrDefaultAsync(Person => Person.Id == command.Input.Id)
                    .ConfigureAwait(false);

                if (person == null)
                {
                    _ = ApplyErrorAsync("Pessoa não encontrado.");
                    return null;
                }

                await _PersonRepository.CommitTransactionAsync();

                return new GetPersonOutput
                {
                    Id = person.Id,
                    FullName = person.FullName,
                    BirthDate = person.BirthDate.Value.ToString("yyyy-MM-dd"),
                    IncomeValue = person.IncomeValue.Value.ToString("N2"),
                    Cpf = person.Cpf,
                    CreateDate = person.CreateDate.ToString("dd/MM/yyyy HH:mm"),
                    UpdateDate = person.UpdateDate?.ToString("dd/MM/yyyy HH:mm") ?? "-"
                };
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

using Application.Boudary.Person;
using Application.Command.Person;
using Domain.Entity.Person;
using Infrastructure.Message;
using Infrastructure.Message.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Repository;

namespace Application.Handler.Person
{
    public class GetPersonsHandler : IRequestHandler<GetPersonsCommand, GetPersonsOutput>
    {
        private readonly IMessagesHandler _messagesHandler;
        private readonly GenericRepository<PersonEntity, int> _PersonRepository;

        public GetPersonsHandler(IMessagesHandler messagesHandler,
                           GenericRepository<PersonEntity, int> PersonRepository)
        {
            _messagesHandler = messagesHandler;
            _PersonRepository = PersonRepository;
        }

        public async Task<GetPersonsOutput> Handle(GetPersonsCommand command, CancellationToken cancellationToken)
        {
            if (command.IsValid())
            {
                await _PersonRepository
                       .BeginTransactionAsync(true)
                       .ConfigureAwait(false);

                var startIndex = command?.Input?.StartIndex ?? 0;
                var length = command?.Input?.PageLength ?? 5;

                var query = _PersonRepository
                    .DbSet
                    .FromSqlRaw($"SELECT * FROM Person WHERE FullName LIKE '%{command.Input.Search}%'")
                    .AsQueryable();

                var total = await query
                    .CountAsync()
                    .ConfigureAwait(false);

                var persons = await query
                    .Skip(startIndex)
                    .Take(length)
                    .ToListAsync()
                    .ConfigureAwait(false);

                //if (!(persons?.Any() ?? false))
                //{
                //    _ = ApplyErrorAsync("Pessoas não encontradas.");
                //    return null;
                //}

                await _PersonRepository.CommitTransactionAsync();

                return new GetPersonsOutput
                {
                    Draw = command.Input.Draw.Value,
                    RecordsTotal = total,
                    RecordsFiltered = total,
                    Data = persons.Select(person => new string[]
                    {
                        person.Id.ToString(),
                        person.FullName,
                        person.BirthDate.Value.ToString("dd/MM/yyyy"),
                        person.IncomeValue.Value.ToString("N2"),
                        person.Cpf,
                        person.CreateDate.ToString("dd/MM/yyyy HH:mm"),
                        person.UpdateDate?.ToString("dd/MM/yyyy HH:mm") ?? "-"
                    })
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

using FluentValidation;

namespace Application.Command.Person.Validation
{
    public class GetPersonsCommandValidation : AbstractValidator<GetPersonsCommand>
    {
        public GetPersonsCommandValidation()
        {
            RuleFor(d => d.Input.Draw)
                .NotEmpty().WithMessage("Dados de paginação devem ser informados")
                .GreaterThan(0).WithMessage("Dados de paginação devem ser positivos");
        }
    }
}

using FluentValidation;

namespace Application.Command.Person.Validation
{
    public class GetPersonCommandValidation : AbstractValidator<GetPersonCommand>
    {
        public GetPersonCommandValidation()
        {
            RuleFor(d => d.Input.Id)
                .NotEmpty().WithMessage("Dados incorretos")
                .GreaterThan(0).WithMessage("Dados incorretos");
        }
    }
}

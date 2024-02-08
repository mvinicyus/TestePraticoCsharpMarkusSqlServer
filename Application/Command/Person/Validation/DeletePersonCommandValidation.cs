using FluentValidation;

namespace Application.Command.Person.Validation
{
    public class DeletePersonCommandValidation : AbstractValidator<DeletePersonCommand>
    {
        public DeletePersonCommandValidation()
        {
            RuleFor(d => d.Input.Id)
                .NotEmpty().WithMessage("Dados incorretos")
                .GreaterThan(0).WithMessage("Dados incorretos");
        }
    }
}

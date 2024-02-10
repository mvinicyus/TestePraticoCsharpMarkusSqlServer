using FluentValidation;
using Application.Utils;

namespace Application.Command.Person.Validation
{
    public class UpdatePersonCommandValidation : AbstractValidator<UpdatePersonCommand>
    {
        public UpdatePersonCommandValidation()
        {
            RuleFor(d => d.Input.Id)
                .NotEmpty().WithMessage("Dados incorretos")
                .GreaterThan(0).WithMessage("Dados incorretos");

            RuleFor(d => d.Input.FullName)
                .NotEmpty().WithMessage("Preencha o nome completo")
                .MaximumLength(150).WithMessage("Nome completo deve ter no máximo 150 caracteres")
                .MinimumLength(10).WithMessage("Nome completo deve ter no mínimo 10 caracteres");

            RuleFor(d => d.Input.BirthDate)
                .NotNull().WithMessage("Preencha a data de nascimento")
                .Must(x =>
                {
                    if (string.IsNullOrWhiteSpace(x))
                    {
                        return false;
                    }
                    DateTime test;
                    var valid = DateTime.TryParse(x, out test) && test > DateTime.MinValue;
                    if (!valid)
                    {
                        return false;
                    }
                    var isPast = test < DateTime.Now.Date;
                    return isPast;
                }).WithMessage("Data de nascimento precisa ser válida")
                .Must(x =>
                {
                    if (string.IsNullOrWhiteSpace(x))
                    {
                        return false;
                    }
                    DateTime test;
                    var valid = DateTime.TryParse(x, out test) && test > DateTime.MinValue;
                    if (!valid)
                    {
                        return false;
                    }
                    var isMajor = test <= DateTime.Now.Date.AddYears(-18);
                    return isMajor;
                })
                .WithMessage("Não é permitido cadastrar pessoas menores de idade");

            RuleFor(d => d.Input.IncomeValue)
                .NotNull().WithMessage("Preencha o valor da renda")
                .Must(x =>
                {
                    decimal test = 0;
                    return decimal.TryParse(x, out test) && test > 0;
                }).WithMessage("Valor da renda precisa ser um número positivo");

            RuleFor(d => d.Input.Cpf)
                .NotNull().WithMessage("Preencha o cpf")
                .NotEmpty().WithMessage("Preencha o cpf")
                .Must(x => x.IsCpf()).WithMessage("O cpf informado é inválido");
        }
    }
}

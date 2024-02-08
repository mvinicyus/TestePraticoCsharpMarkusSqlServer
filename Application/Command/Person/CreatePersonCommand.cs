using Application.Boudary.Person;
using Application.Command.Person.Validation;
using Infrastructure.Message;
using System.ComponentModel.DataAnnotations;

namespace Application.Command.Person
{
    public class CreatePersonCommand : Command<CreatePersonOutput>
    {
        public CreatePersonCommand() { }

        public CreatePersonInput Input { get; set; }
        public override bool IsValid()
        {
            ValidationResult = new CreatePersonCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}

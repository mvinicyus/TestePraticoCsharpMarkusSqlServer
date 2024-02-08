using Application.Boudary.Person;
using Application.Command.Person.Validation;
using Infrastructure.Message;
using System.ComponentModel.DataAnnotations;

namespace Application.Command.Person
{
    public class DeletePersonCommand : Command<DeletePersonOutput>
    {
        public DeletePersonCommand() { }

        public DeletePersonInput Input { get; set; }
        public override bool IsValid()
        {
            ValidationResult = new DeletePersonCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}

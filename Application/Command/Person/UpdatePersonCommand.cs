using Application.Boudary.Person;
using Application.Command.Person.Validation;
using Infrastructure.Message;
using System.ComponentModel.DataAnnotations;

namespace Application.Command.Person
{
    public class UpdatePersonCommand : Command<UpdatePersonOutput>
    {
        public UpdatePersonCommand() { }

        public UpdatePersonInput Input { get; set; }
        public override bool IsValid()
        {
            ValidationResult = new UpdatePersonCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}

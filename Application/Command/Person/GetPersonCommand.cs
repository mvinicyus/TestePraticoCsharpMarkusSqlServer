using Application.Boudary.Person;
using Application.Command.Person.Validation;
using Infrastructure.Message;
using System.ComponentModel.DataAnnotations;

namespace Application.Command.Person
{
    public class GetPersonCommand : Command<GetPersonOutput>
    {
        public GetPersonCommand() { }

        public GetPersonInput Input { get; set; }
        public override bool IsValid()
        {
            ValidationResult = new GetPersonCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}

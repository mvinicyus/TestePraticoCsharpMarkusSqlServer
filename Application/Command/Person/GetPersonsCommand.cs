using Application.Boudary.Person;
using Application.Command.Person.Validation;
using Infrastructure.Message;
using System.ComponentModel.DataAnnotations;

namespace Application.Command.Person
{
    public class GetPersonsCommand : Command<GetPersonsOutput>
    {
        public GetPersonsCommand() { }

        public GetPersonsInput Input { get; set; }
        public override bool IsValid()
        {
            ValidationResult = new GetPersonsCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}

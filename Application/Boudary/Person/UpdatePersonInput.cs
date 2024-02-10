namespace Application.Boudary.Person
{
    public class UpdatePersonInput
    {
        public int? Id { get; set; }
        public string? FullName { get; set; }
        public string? BirthDate { get; set; }
        public string? IncomeValue { get; set; }
        public string? Cpf { get; set; }
    }
}

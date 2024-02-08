namespace Application.Boudary.Person
{
    public class UpdatePersonInput
    {
        public int? Id { get; set; }
        public string? FullName { get; set; }
        public DateTime? BirthDate { get; set; }
        public decimal? IncomeValue { get; set; }
        public string? Cpf { get; set; }
    }
}

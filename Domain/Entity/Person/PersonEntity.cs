using Domain.Entity.Base;

namespace Domain.Entity.Person
{
    public class PersonEntity : AggregateRoot<int>
    {
        public string? FullName { get; set; }
        public DateTime? BirthDate { get; set; }
        public decimal? IncomeValue { get; set; }
        public string? Cpf { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}

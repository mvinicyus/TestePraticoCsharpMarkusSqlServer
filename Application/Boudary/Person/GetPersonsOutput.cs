
namespace Application.Boudary.Person
{
    public class GetPersonsOutput
    {
        public int Draw { get; set; }
        public int TotalItens { get; set; }
        public IEnumerable<PersonInfoOutput> Data { get; set; }
    }
}
